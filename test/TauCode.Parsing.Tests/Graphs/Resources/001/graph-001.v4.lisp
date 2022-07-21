(group :name "main"
    (vertex :name "a")

    (group :name "inner"
        (vertex :name "b" :links-from ("/main/a"))
    )
)