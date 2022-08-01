using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.TinyLisp
{
    public static class TinyLispExtensions
    {
        public static IList<Tuple<int, Keyword, Element>> GetAllKeywordArguments(
            this Element element)
        {
            var pseudoList = CheckElementIsPseudoList(element, nameof(element), true);

            var tuples = new List<Tuple<int, Keyword, Element>>();

            for (var i = 0; i < pseudoList.Count; i++)
            {
                var listElement = pseudoList[i];
                if (listElement is Keyword keyword)
                {
                    if (i == pseudoList.Count - 1)
                    {
                        throw new TinyLispException("Keyword is at the end of the list.");
                    }

                    var nextElement = pseudoList[i + 1];
                    if (nextElement is Keyword nextKeyword)
                    {
                        throw new TinyLispException("Two keywords in a row.");
                    }

                    var tuple = Tuple.Create(i, keyword, nextElement);
                    tuples.Add(tuple);

                    i++; // sic. skip keyword's argument and move on.
                }
            }

            return tuples;
        }

        public static Element? GetSingleKeywordArgument(
            this Element element,
            string argumentName,
            bool absenceIsAllowed = false)
        {
            if (argumentName == null)
            {
                throw new ArgumentNullException(nameof(argumentName));
            }

            var pseudoList = CheckElementIsPseudoList(element, nameof(element), true);

            if (!TinyLispHelper.IsValidSymbolName(argumentName, true))
            {
                throw new ArgumentException($"'{argumentName}' is not a valid keyword.", nameof(argumentName));
            }

            var wantedKeyword = Symbol.Create(argumentName);
            int wantedIndex;
            var index = pseudoList.FindFirstIndex(wantedKeyword);

            if (index < 0)
            {
                if (absenceIsAllowed)
                {
                    return null;
                }
                else
                {
                    throw new TinyLispException($"No argument for keyword '{argumentName}'.");
                }
            }
            else
            {
                wantedIndex = index + 1;
            }

            if (wantedIndex == pseudoList.Count)
            {
                throw new TinyLispException(
                    $"Keyword '{argumentName}' was found, but at the end of the list.");
            }

            var wantedElement = pseudoList[wantedIndex];
            if (wantedElement is Keyword)
            {
                throw new TinyLispException(
                    $"Keyword '{argumentName}' was found, but next element is a keyword too.");
            }

            return wantedElement;
        }

        public static TElement? GetSingleKeywordArgument<TElement>(
            this Element element,
            string argumentName,
            bool absenceIsAllowed = false) where TElement : Element
        {
            var argument = element.GetSingleKeywordArgument(argumentName, absenceIsAllowed);
            if (argument == null)
            {
                return null;
            }

            if (argument is TElement expectedElement)
            {
                return expectedElement;
            }

            throw new TinyLispException(
                $"Argument for '{argumentName}' was found, but it appears to be of type '{argument.GetType().FullName}' instead of expected type '{typeof(TElement).FullName}'.");
        }

        public static PseudoList GetAllKeywordArguments(
            this Element element,
            string argumentName,
            bool absenceIsAllowed = false)
        {
            var pseudoList = CheckElementIsPseudoList(element, nameof(element), true);

            if (argumentName == null)
            {
                throw new ArgumentNullException(nameof(argumentName));
            }

            if (!TinyLispHelper.IsValidSymbolName(argumentName, true))
            {
                throw new ArgumentException($"'{argumentName}' is not a valid keyword.", nameof(argumentName));
            }

            var wantedKeyword = Symbol.Create(argumentName);
            var index = pseudoList.FindFirstIndex(wantedKeyword);

            if (index == -1)
            {
                if (absenceIsAllowed)
                {
                    return new PseudoList(); // empty
                }
                else
                {
                    throw new TinyLispException($"No argument for keyword '{argumentName}'.");
                }
            }

            index++; // move forward from keyword.
            var startIndex = index;

            while (true)
            {
                if (index == pseudoList.Count)
                {
                    break;
                }

                var listElement = pseudoList[index];
                if (listElement is Keyword)
                {
                    break;
                }

                index++;
            }

            var lastIndex = index - 1;

            var result = new PseudoList();

            for (var i = startIndex; i <= lastIndex; i++)
            {
                result.Add(pseudoList[i]);
            }

            return result;
        }

        public static bool? GetSingleArgumentAsBool(this Element element, string argumentName)
        {
            var pseudoList = CheckElementIsPseudoList(element, nameof(element), true);

            var argument = pseudoList.GetSingleKeywordArgument(argumentName, true);
            if (argument == null)
            {
                return null;
            }

            if (argument == True.Instance)
            {
                return true;
            }

            if (argument == Nil.Instance)
            {
                return false;
            }

            throw new TinyLispException(
                $"Keyword '{argumentName}' was found, but it appeared to be '{argument}' instead of NIL or T.");
        }

        public static PseudoList GetFreeArguments(this Element element)
        {
            var freeArgumentSets = element.GetMultipleFreeArgumentSets();
            if (freeArgumentSets.Count == 0)
            {
                throw new TinyLispException("Free arguments not found.");
            }

            if (freeArgumentSets.Count > 1)
            {
                throw new TinyLispException("More than one set of free arguments was found.");
            }

            return freeArgumentSets[0];
        }

        public static IList<PseudoList> GetMultipleFreeArgumentSets(this Element element)
        {
            var pseudoList = CheckElementIsPseudoList(element, nameof(element), true);

            var index = 1;
            var result = new List<PseudoList>();

            var startedWithKeyword = false;
            var startIndex = 1;

            while (true)
            {
                if (index == pseudoList.Count)
                {
                    if (startIndex == -1)
                    {
                        // nothing to add.
                    }
                    else
                    {
                        // got free args.
                        var firstArgIndex = startIndex;

                        if (startedWithKeyword)
                        {
                            firstArgIndex++;
                        }

                        var lastArgIndex = index - 1;

                        if (firstArgIndex <= lastArgIndex)
                        {
                            var freeArgsPseudoList = new PseudoList();
                            for (var i = firstArgIndex; i <= lastArgIndex; i++)
                            {
                                freeArgsPseudoList.Add(pseudoList[i]);
                            }

                            result.Add(freeArgsPseudoList);
                        }
                    }

                    break;
                }

                var listElement = pseudoList[index];
                if (listElement is Keyword)
                {
                    // bumped into keyword
                    if (startIndex == -1)
                    {
                        // reset again.
                        startedWithKeyword = true;
                        index++;
                    }
                    else
                    {
                        // was started, let's check, maybe we can deliver pseudo-list of free args.
                        var delta = index - startIndex;
                        if (
                            delta == 0 ||
                            (delta == 1 && startedWithKeyword)
                        )
                        {
                            // won't consider it free.
                            // reset the entire procedure.
                            startedWithKeyword = true;
                            startIndex = -1;
                            index++;
                        }
                        else
                        {
                            // got free args!
                            var firstArgIndex = startIndex;

                            if (startedWithKeyword)
                            {
                                firstArgIndex++;
                            }

                            var lastArgIndex = index - 1;

                            var freeArgsPseudoList = new PseudoList();
                            for (var i = firstArgIndex; i <= lastArgIndex; i++)
                            {
                                freeArgsPseudoList.Add(pseudoList[i]);
                            }

                            result.Add(freeArgsPseudoList);

                            startIndex = -1;
                            index++;

                            startedWithKeyword = true;
                        }
                    }
                }
                else
                {
                    // bumped into non-keyword.
                    if (startIndex == -1)
                    {
                        // let's start.
                        startIndex = index;
                        index++;
                    }
                    else
                    {
                        // was started, bumped into non-keyword again. good, let's continue.
                        index++;
                    }
                }
            }

            return result;
        }

        public static Element GetCar(this Element element)
        {
            var pseudoList = CheckElementIsPseudoList(element, nameof(element), true);

            return pseudoList[0];
        }

        public static TElement GetCar<TElement>(this Element element) where TElement : Element
        {
            var car = element.GetCar();
            if (car is TElement castElement)
            {
                return castElement;
            }

            throw new ArgumentException(
                $"Argument is expected to be of type '{typeof(TElement).FullName}', but was of type '{element.GetType().FullName}'.",
                nameof(element));
        }

        public static PseudoList AsPseudoList(this Element element) =>
            element.AsElement<PseudoList>();

        public static TElement AsElement<TElement>(this Element? element) where TElement : Element
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (element is TElement wantedElement)
            {
                return wantedElement;
            }

            throw new ArgumentException(
                $"Argument is expected to be of type '{typeof(TElement).FullName}', but was of type '{element.GetType().FullName}'.",
                nameof(element));
        }

        public static bool ToBool(this Element element)
        {
            // todo checks
            if (element is True)
            {
                return true;
            }
            else if (element is Nil)
            {
                return false;
            }

            throw new ArgumentException("Element cannot be converted to 'bool'.");
        }

        private static PseudoList CheckElementIsPseudoList(Element element, string argumentName, bool mustBeNotEmpty)
        {
            if (element == null!)
            {
                throw new ArgumentNullException(argumentName);
            }

            if (element is PseudoList pseudoList)
            {
                if (mustBeNotEmpty && pseudoList.Count == 0)
                {
                    throw new TinyLispException("Pseudo list is empty.");
                }

                return pseudoList;
            }

            throw new ArgumentException(
                $"'{argumentName}' is expected to be of type '{typeof(PseudoList).FullName}'.",
                argumentName);
        }
    }
}
