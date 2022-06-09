using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs
{
    public static class GraphExtensions
    {
        public static IPartMold ResolvePath(this IPartMold partMold, string path)
        {
            // todo checks
            if (path.StartsWith("/"))
            {
                throw new NotImplementedException();
            }

            var parts = path.Split('/');

            IGroupMold currentGroup;

            if (partMold is IGroupMold groupMold)
            {
                currentGroup = groupMold;
            }
            else if (partMold is IVertexMold vertexMold)
            {
                currentGroup = vertexMold.Owner;
            }
            else
            {
                throw new NotImplementedException();
            }

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];

                if (part == ".")
                {
                    // 'currentGroup' doesn't change
                }
                else if (part == "..")
                {
                    currentGroup = currentGroup.Owner;
                    if (currentGroup == null)
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (part == "")
                {
                    throw new NotImplementedException();
                }
                else
                {
                    // todo: check part is a valid name
                    var child = currentGroup.Content.SingleOrDefault(x => x.Name == part);
                    // todo: check name uniqueness; todo: forbid name change?

                    if (child == null)
                    {
                        throw new NotImplementedException();
                    }

                    if (child is IVertexMold vertexMold)
                    {
                        if (i == parts.Length - 1)
                        {
                            return vertexMold;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
