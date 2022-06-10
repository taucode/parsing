(group
	:name "main"

	(sequence
		:name "create"

		("CREATE")

		(alternatives
			:name "create-alternatives"

			(group-ref :group-path "../../create-table/")
			(group-ref :group-path "../../create-index/")
		)

		(end)
	)

	(sequence
		:name "create-table"

		("TABLE" :name "do-create-table")
		(identifier :name "table-name")
		("(")
		(group-ref
			:group-path "../column-def/"
			:name "column-definition-ref"
			:links-to ("table-closing")
		)
		("," :links-to ("column-definition-ref"))
		(group-ref :group-path "../constraint-definitions/")
		(")" :name "table-closing")
	)

	(sequence
		:name "column-def"

		(identifier :name "column-name")
		(identifier :name "type-name")
		(optional
			(sequence
				("(")
				(integer :name "precision")
				(optional
					(sequence
						(",")
						(integer :name "scale")
					)
				)
				(")")
			)
		)
		(optional
			(alternatives
				("NULL" :name "null")
				(sequence
					("NOT")
					("NULL" :name "not-null")
				)
			)
		)
		(optional
			(sequence
				("PRIMARY")
				("KEY" :name "inline-primary-key")
			)
		)
		(optional
			(sequence
				("DEFAULT")
				(alternatives
					("NULL" :name "default-null")
					(integer :name "default-integer")
					(string :name "default-string")
				)
			)
		)
	)

	(sequence
		:name "constraint-defs"

		("CONSTRAINT" :name "constraint")
		(identifier :name "constraint-name")
		(alternatives
			(group-ref :group-path "../../primary-key/")
			(group-ref :group-path "../../foreign-key/")
		)
		(splitter
			(idle :is-entrance t)

			("," :links-to ("constraint"))
			(idle) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
	)

	(sequence
		:name "primary-key"

		("PRIMARY" :name "do-primary-key")
		("KEY")
		(group-ref :group-path "../pk-columns/")
	)

	(sequence
		:name "pk-columns"

		("(")
		(identifier :name "pk-column-name")
		(optional
			(multi-text :@values ("ASC" "DESC") :name "pk-asc-or-desc")
		)
		(splitter
			:name "more-pk-columns"

			(idle :is-entrance t)
			("," :links-to ("pk-column-name"))
			(idle) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")")
	)

	(sequence
		:name foreign-key

		("FOREIGN" :name do-foreign-key)
		("KEY")
		(group-ref :group-path "../fk-columns/")
		("REFERENCES")
		(identifier :name "fk-referenced-table-name")
		(group-ref :group-path "../fk-referenced-columns/")
	)

	(sequence
		:name fk-columns

		("(")
		(identifier :name fk-column-name)
		(splitter
			(idle :is-entrance t)

			("," :links-to "../fk-column-name")
			(idle) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")")
	)

	(sequence
		:name "fk-referenced-columns"

		("(")
		(identifier :name fk-referenced-column-name)
		(splitter
			(idle :is-entrance t)

			("," :links-to ("../fk-referenced-column-name"))
			(idle) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(exact-punctuation :value ")")
	)

	(sequence
		:name "create-index"

		(optional
			("UNIQUE" :name "do-create-unique-index")
		)
		("INDEX" :name "do-create-index")
		(identifier :name "index-name")
		("ON")
		(identifier :name "index-table-name")
		("(")
		(identifier :name "index-column-name")
		(optional
			(multi-text :values ("ASC" "DESC") :name "index-column-asc-or-desc"))
		(splitter
			(idle :is-entrance t)

			(:value "," :links-to ("../index-column-name"))
			(idle) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")")
	)
)
