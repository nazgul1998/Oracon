# ORACON Aula Virtual



## El presente proyecto fue desarrollado para la firma contable ORACON, consiste en un aula virtual


### Nota: El script de base de datos se encuentra en la carpeta Models>ScriptDB, y la cadena de conexión se encuentra en appsettings.json (DevConnection) y el token para encriptar contraseñas "token"


## Al ingresar a la web veremos todos los cursos registrados como invitado, es decir no podemos adquirir ningún curso, hasta crearnos una cuenta.

![Image text](./md/invitado-home.gif)


## Luego el usuario se registra e inicia sesión, tras esto el usuario vera que se agregara la opción de agregar en cada fila de la tabla de cursos, tras seleccionar un curso procedemos a registrarnos, esto nos permitirá estar inscrito en el curso mas no ver el contenido. Este pago se realiza en las oficinas de la empresa. 

![Image text](./md/usuario-registro_login_y_inscripcion_en_curso.gif)


## Tras a ver realizado el pago del curso, el administrador cambiara el estado de su matrícula a "pagado", aquí podrá tener acceso al contenido  del curso, que se mostrara de la siguiente manera. 
![Image text](./md/usuario-mi_aprendizaje_conetnido.gif)


## El administrador gestionara los profesores, cursos y pagos, como se muestra a continuación. 

![Image text](./md/admin-cursos_docentes_y_pagos.gif)


## El docente podrá gestionar los cursos a los cuales fue asignado, agregando a estos sus módulos y dentro de este, material multimedia. (documentos, crear tareas, agregar enlaces (zoom, etc.), etc.

![Image text](./md/docente-gestion_de_curso.gif)


## Finalmente el docente puede revisar las tareas enviadas. 

![Image text](./md/docente-revisar_entrega.gif)

