(group :name top
    (sequence :name "top-seq"
        (vertex
            :name "sd"
            :type "term"
            :@term "sd"
        )
        (splitter :name "switches"
            (vertex
                :type "idle"
                :name "switches"
                :is-entrance t
            )

            (sequence :name "connection-route"
                (vertex
                    :type "key"
                    :@keys (
                        "-c"
                        "--connection"
                    )
                    :@alias "connection"
                )
                (vertex
                    :type "key-value"
                    :@alias "connection"
                    :links-to (
                        "../idle"
                    )
                )
            )

            (sequence :name "provider-route"
                (vertex
                    :type "key"
                    :@keys (
                        "-p"
                        "--provider"
                    )
                    :@alias "provider"
                )
                (vertex
                    :type "key-value"
                    :@alias "provider"
                    :links-to (
                        "../idle"
                    )                    
                )
            )

            (sequence :name "file-route"
                (vertex
                    :type "key"
                    :@keys (
                        "-f"
                        "--file"
                    )
                    :@alias "file"
                )
                (vertex
                    :type "key-value"
                    :@alias "file"
                    :links-to (
                        "../idle"
                    )
                )
            )

            (vertex
                :type "end"
                :is-exit t
            )
        )
    )
)
