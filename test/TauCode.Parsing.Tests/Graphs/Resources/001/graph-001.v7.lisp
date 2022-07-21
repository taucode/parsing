(group :name "main"
    (vertex :name "a" :links-to("my-ref"))
    (ref :name "my-ref" :path "other-group/")

    (group :name "other-group"
        (vertex :name "b" :is-entrance t)
    )
)