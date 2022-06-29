(group :name "main"
    (vertex :name "a" :links-to ("inner/b"))

    (group :name "inner"
        (vertex :name "b")
    )
)