(group
	:name "main"

	(sequence
		;:name "create"

		(idle
			:name "root-node"
			:links-to ("end-of-clause"))

		("CREATE" :name "create")

		(alternatives
			:name "create-alternatives"

			(ref :path "../../create-table/")
			(ref :path "../../create-index/")
		)

		(end :name "end-of-clause")
	)

	(sequence
		:name "create-table"

		("TABLE" :name "do-create-table")
		(identifier :name "table-name")
		("(" :name "table-opening")
		(ref
			:path "../column-def/"
			:name "column-definition-ref"
			:links-to ("table-closing")
		)
		("," :links-to ("column-definition-ref"))
		(ref
			:path "../constraint-defs/"
		)
		(")" :name "table-closing")
	)

	(sequence
		:name "column-def"

		(identifier :name "column-name")
		(identifier :name "type-name")
		(optional :name "optional-precision"
			(sequence
				("(")
				(integer :name "precision")
				(optional :name "optional-scale"
					(sequence
						(",")
						(integer :name "scale")
					)
				)
				(")")
			)
		)
		(optional :name "nullability"
			(alternatives :name "null-or-not-null"
				("NULL" :name "null")
				(sequence
					("NOT")
					("NULL" :name "not-null")
				)
			)
		)
		(optional :name "optional-inline-primary-key"
			(sequence
				("PRIMARY")
				("KEY" :name "inline-primary-key")
			)
		)
		(optional :is-exit t
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
			(ref :path "../../primary-key/")
			(ref :path "../../foreign-key/")
		)
		(splitter :is-exit t
			(idle :is-entrance t)

			("," :links-to ("../constraint"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
	)

	(sequence
		:name "primary-key"

		("PRIMARY" :name "do-primary-key")
		("KEY")
		(ref :path "../pk-columns/")
	)

	(sequence
		:name "pk-columns"

		("(")
		(identifier :name "pk-column-name")
		(optional
			(multi-word :values ("ASC" "DESC") :name "pk-asc-or-desc")
		)
		(splitter
			:name "more-pk-columns"

			(idle :is-entrance t)
			("," :links-to ("../pk-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")")
	)

	(sequence
		:name "foreign-key"

		("FOREIGN" :name "do-foreign-key")
		("KEY")
		(ref :path "../fk-columns/")
		("REFERENCES")
		(identifier :name "fk-referenced-table-name")
		(ref :path "../fk-referenced-columns/")
	)

	(sequence
		:name "fk-columns"

		("(")
		(identifier :name "fk-column-name")
		(splitter
			(idle :is-entrance t)

			("," :links-to ("../fk-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")")
	)

	(sequence
		:name "fk-referenced-columns"

		("(")
		(identifier :name "fk-referenced-column-name")
		(splitter
			(idle :is-entrance t)

			("," :links-to ("../fk-referenced-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")")
	)

	(sequence
		:name "create-index"

		(alternatives
			(sequence
				("UNIQUE")
				("INDEX" :name "do-create-unique-index")
			)
			("INDEX" :name "do-create-non-unique-index")
		)

		(identifier :name "index-name")
		("ON")
		(identifier :name "index-table-name")
		("(")
		(identifier :name "index-column-name")
		(optional
			(multi-word :values ("ASC" "DESC") :name "index-column-asc-or-desc"))
		(splitter
			(idle :is-entrance t)

			("," :links-to ("../index-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")")
	)
)
