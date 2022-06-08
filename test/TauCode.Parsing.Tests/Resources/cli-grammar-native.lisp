﻿(sequence 
    (vertex
        :name "root"
        :type "term"
        :@term "sd"
    )
    (splitter :name "switches"
        (vertex
            :type "idle"
            :name "idle"
            :is-entrance t
        )

        (sequence :name "connection-route"
            (vertex
                :name "connection-key"
                :type "key"
                :@keys (
                    "-c"
                    "--connection"
                )
                :@alias "connection"
                :@is-unique t
            )
            (vertex
                :name "connection-value"
                :type "key-value"
                :@alias "connection"
                :links-to (
                    "../idle"
                )
            )
        )

        (sequence :name "provider-route"
            (vertex
                :name "provider-key"
                :type "key"
                :@keys (
                    "-p"
                    "--provider"
                )
                :@alias "provider"
                :@is-unique t
            )
            (vertex
                :name "provider-value"
                :type "key-value"
                :@alias "provider"
                :links-to (
                    "../idle"
                )                    
            )
        )

        (sequence :name "file-route"
            (vertex
                :name "file-key"
                :type "key"
                :@keys (
                    "-f"
                    "--file"
                )
                :@alias "file"
                :@is-unique t
            )
            (vertex
                :name "file-value"
                :type "key-value"
                :@alias "file"
                :links-to (
                    "../idle"
                )
            )
        )

        (vertex
            :type "end"
            :name "end"
            :is-exit t
        )
    )
)
