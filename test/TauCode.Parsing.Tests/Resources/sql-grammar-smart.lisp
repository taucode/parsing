(group
	:name "main"

	(sequence
		:name "create"

		("CREATE" :is-entrance t)

		(alternatives
			:name "create-alternatives"

			(group-ref :group-path "../../create-table/")
			(group-ref :group-path "../../create-index/")
		)

		(end :is-exit t)
	)

	(sequence
		:name "create-table"

		("TABLE" :name "do-create-table" :is-entrance t)
		(identifier :name "table-name")
		("(")
		(group-ref
			:group-path "../column-def/"
			:name "column-definition-ref"
			:links-to ("table-closing")
		)
		("," :links-to ("column-definition-ref"))
		(group-ref :group-path "../constraint-definitions/")
		(")" :name "table-closing" :is-exit t)
	)

	(sequence
		:name "column-def"

		(identifier :name "column-name" :is-entrance t)
		(identifier :name "type-name")
		(optional
			(sequence
				("(" :is-entrance t)
				(integer :name "precision")
				(optional
					(sequence
						("," :is-entrance t)
						(integer :name "scale" :is-exit t)
					)
				)
				(")" :is-exit t)
			)
		)
		(optional
			(alternatives
				("NULL" :name "null")
				(sequence
					("NOT" :is-entrance t)
					("NULL" :name "not-null" :is-exit t)
				)
			)
		)
		(optional
			(sequence
				("PRIMARY" :is-entrance t)
				("KEY" :name "inline-primary-key" :is-exit t)
			)
		)
		(optional :is-exit t
			(sequence
				("DEFAULT" :is-entrance t)
				(alternatives :is-exit t
					("NULL" :name "default-null")
					(integer :name "default-integer")
					(string :name "default-string")
				)
			)
		)
	)

	(sequence
		:name "constraint-defs"

		("CONSTRAINT" :name "constraint" :is-entrance t)
		(identifier :name "constraint-name")
		(alternatives
			(group-ref :group-path "../../primary-key/")
			(group-ref :group-path "../../foreign-key/")
		)
		(splitter :is-exit t
			(idle :is-entrance t)

			("," :links-to ("../constraint"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
	)

	(sequence
		:name "primary-key"

		("PRIMARY" :name "do-primary-key" :is-entrance t)
		("KEY")
		(group-ref :group-path "../pk-columns/" :is-exit t)
	)

	(sequence
		:name "pk-columns"

		("(" :is-entrance t)
		(identifier :name "pk-column-name")
		(optional
			(multi-text :@values ("ASC" "DESC") :name "pk-asc-or-desc")
		)
		(splitter
			:name "more-pk-columns"

			(idle :is-entrance t)
			("," :links-to ("pk-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")" :is-exit t)
	)

	(sequence
		:name "foreign-key"

		("FOREIGN" :name "do-foreign-key" :is-entrance t)
		("KEY")
		(group-ref :group-path "../fk-columns/")
		("REFERENCES")
		(identifier :name "fk-referenced-table-name")
		(group-ref :group-path "../fk-referenced-columns/" :is-exit t)
	)

	(sequence
		:name "fk-columns"

		("(" :is-entrance t)
		(identifier :name "fk-column-name")
		(splitter
			(idle :is-entrance t)

			("," :links-to ("../fk-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")" :is-exit t)
	)

	(sequence
		:name "fk-referenced-columns"

		("(" :is-entrance t)
		(identifier :name "fk-referenced-column-name")
		(splitter
			(idle :is-entrance t)

			("," :links-to ("../fk-referenced-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")" :is-exit t)
	)

	(sequence
		:name "create-index"

		(optional :is-entrance t
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

			("," :links-to ("../index-column-name"))
			(idle :is-exit t) ;;;;;; NB: _not_ joint! :is-exit is nil here.
		)
		(")" :is-exit t)
	)
)
