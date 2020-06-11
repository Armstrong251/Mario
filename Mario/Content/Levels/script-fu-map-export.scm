(define (strlen str) (length (string->list str)))
(define (script-fu-map-export image drawable directory)
    (for-each (lambda (list) 
      (let* 
        (
          (item (car list))
          (layername (cadr list))
          (filename 
            (strcat directory DIR-SEPARATOR
              (if (= (strcmp (substring layername (- (strlen layername) 4)) ".png") 0)
                layername
                (strcat layername ".png")
              )
            )
          )
        )
        (file-png-save-defaults
          RUN-NONINTERACTIVE
          image
          item
          filename
          filename
        )
      )
    )
    (map (lambda (x) (list x (car (gimp-item-get-name x))))
      (vector->list (cadr (gimp-image-get-layers image)))
    )
  )
  (gimp-displays-flush)
)
(script-fu-register
    "script-fu-map-export"                        ;func name
    "Export Map"                                  ;menu label
    ""              ;description
    "Will Kennedy"                             ;author
    ""        ;copyright notice
    ""                          ;date created
    ""                     ;image type that the script works on
    SF-IMAGE "SF-IMAGE" 0
    SF-DRAWABLE "SF-DRAWABLE" 0
    SF-DIRNAME "Export to..." "~/"
)
(script-fu-menu-register "script-fu-map-export" "<Image>/File/Export Map")