(group :name "main"
    (vertex :name "a" :links-to ("other-group/"))

    (group :name "other-group"
        (vertex :name "b" :is-entrance t)
    )
)