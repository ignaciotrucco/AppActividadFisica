window.onload = ListadoTipoEjercicios();

function ListadoTipoEjercicios() {
    $.ajax({
        //URL PARA PETICION
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {},
        //especifico peticion tipo POST
        type: 'POST',
        //info que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (tipoDeEjercicios) {

            $("#ModalTipoEjercicio").modal("hide");
            LimpiarModal();
            //$("#tbody-tipoejercicios").empty();
            let contenidoTabla = ``;

            $.each(tipoDeEjercicios, function (index, tipoDeEjercicio) {

                contenidoTabla += `
                <tr>
                    <td>${tipoDeEjercicio.descripcion}</td>
                    <td class="text-center"><button type="button" class="btn btn-success" onclick="AbrirModalEditar(${tipoDeEjercicio.tipoEjercicioID})"><i class="fa-solid fa-pen-to-square"></i></button></td>
                    <td class="text-center"><button type="button" class="btn btn-danger" onclick="EliminarRegistros(${tipoDeEjercicio.tipoEjercicioID})"><i class="fa-solid fa-trash"></i></button></td>
                </tr>
            `;

                //  $("#tbody-tipoejercicios").append(`
                //     <tr>
                //         <td>${tipoDeEjercicio.descripcion}</td>
                //         <td class="text-center"></td>
                //         <td class="text-center"></td>
                //     </tr>
                //  `);
            });

            document.getElementById("tbody-tipoejercicios").innerHTML = contenidoTabla;
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Algo salió mal",
            });
        }
    });
}

function LimpiarModal() {
    document.getElementById("IdEjercicios").value = 0;
    document.getElementById("descripcion").value = "";
}

function NuevoRegistro() {
    $("#tituloModal").text("Nuevo tipo de ejercicio")
}

function AbrirModalEditar(tipoEjercicioID) {

    $.ajax({
        //URL PARA PETICION
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { tipoEjercicioID: tipoEjercicioID },
        //especifico peticion tipo POST
        type: 'POST',
        //info que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (tipoDeEjercicios) {
            let tipoDeEjercicio = tipoDeEjercicios[0]

            document.getElementById("IdEjercicios").value = tipoEjercicioID;
            $("#tituloModal").text("Editar tipo de ejercicio")
            document.getElementById("descripcion").value = tipoDeEjercicio.descripcion;
            $("#ModalTipoEjercicio").modal("show");
        },
        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Algo salió mal",
            });
        }
    });
}

function GuardarRegistro() {
    //GUARDAMOS EN UNA VARIABLE LOS DATOS DEL INPUT
    let tipoEjercicioID = document.getElementById("IdEjercicios").value;
    let descripcion = document.getElementById("descripcion").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO SE NECESITA VERIFICAR EN BASES DE DATOS
    //LUEGO POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS LA BASE DE DATOS.
    console.log(descripcion);
    Swal.fire({
        title: "¿Está seguro que quiere guardar este registro?",
        showDenyButton: true,
        confirmButtonText: "Guardar",
        denyButtonText: `No guardar`
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            $.ajax({
                //URL PARA PETICION
                url: '../../TipoEjercicios/GuardarTipoEjercicios',
                // la información a enviar
                // (también es posible utilizar una cadena de datos)
                data: { tipoEjercicioID: tipoEjercicioID, descripcion: descripcion },
                //especifico peticion tipo POST
                type: 'POST',
                //info que se espera de respuesta
                dataType: 'json',
                // código a ejecutar si la petición es satisfactoria;
                // la respuesta es pasada como argumento a la función
                success: function (resultado) {
                    if (resultado != "") {
                        Swal.fire(resultado);
                    }
                    ListadoTipoEjercicios();
                },
                // código a ejecutar si la petición falla;
                // son pasados como argumentos a la función
                // el objeto de la petición en crudo y código de estatus de la petición
                error: function (xhr, status) {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "Algo salió mal",
                    });
                }
            });
        } else if (result.isDenied) {
            Swal.fire("El elemento no ha sido guardado", "", "info");
            ListadoTipoEjercicios();
        }
    });

}

function EliminarRegistros(tipoEjercicioID) {

    Swal.fire({
        title: "¿Esta seguro que desea eliminar este registro?",
        text: "¡No habrá vuelta atras!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, quiero eliminar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                //URL PARA PETICION
                url: '../../TipoEjercicios/EliminarTipoEjercicio',
                // la información a enviar
                // (también es posible utilizar una cadena de datos)
                data: { tipoEjercicioID: tipoEjercicioID },
                //especifico peticion tipo POST
                type: 'POST',
                //info que se espera de respuesta
                dataType: 'json',
                // código a ejecutar si la petición es satisfactoria;
                // la respuesta es pasada como argumento a la función
                success: function (resultado) {
                    if (resultado) {
                        Swal.fire({
                            title: "¡Hecho!",
                            text: "Su registro ha sido eliminado.",
                            icon: "success"
                        });
                        ListadoTipoEjercicios();
                    }
                },
                // código a ejecutar si la petición falla;
                // son pasados como argumentos a la función
                // el objeto de la petición en crudo y código de estatus de la petición
                error: function (xhr, status) {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "Algo salió mal",
                    });
                }
            });
        }
    });
}

