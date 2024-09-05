window.onload = ListadoLugares();

function LimpiarModal() {
    $("#LugarID").val(0);
    $("#NombreLugar").val("");
}

function ListadoLugares() {
    $.ajax({
        //URL PARA PETICION
        url: '../../Lugares/ListadoLugares',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {},
        //especifico peticion tipo POST
        type: 'POST',
        //info que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (listadoLugares) {

            $("#modalLugares").modal("hide");
            LimpiarModal();

            let contenidoTabla = ``;

            $.each(listadoLugares, function (index, lugar) {

                contenidoTabla += `
                <tr>
                    <td>${lugar.nombre}</td>
                    <td class="text-center"><button type="button" class="btn btn-success" onclick="ModalEditar(${lugar.lugarID})"><i class="fa-solid fa-pen-to-square"></i></button></td>
                    <td class="text-center"><button type="button" class="btn btn-danger" onclick="EliminarLugar(${lugar.lugarID})"><i class="fa-solid fa-trash"></i></button></td>
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

            document.getElementById("tbody-lugares").innerHTML = contenidoTabla;
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

function NuevoLugar() {
    $("#tituloModal").text("Nuevo Lugar");
}

function GuardarLugar() {
    let lugarID = $("#LugarID").val();
    let nombre = $("#NombreLugar").val();
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
                url: '../../Lugares/GuardarLugar',
                // la información a enviar
                // (también es posible utilizar una cadena de datos)
                data: { LugarID: lugarID, Nombre: nombre },
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
                    ListadoLugares();
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

function ModalEditar(lugarID) {

    $("#tituloModal").text("Editar Lugar");

    $.ajax({
        //URL PARA PETICION
        url: '../../Lugares/ListadoLugares',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { LugarID: lugarID },
        //especifico peticion tipo POST
        type: 'POST',
        //info que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (listadoLugares) {
            let lugar = listadoLugares[0]

            $("#LugarID").val(lugarID);
            $("#modalLugares").modal("show");
            $("#NombreLugar").val(lugar.nombre);


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

function EliminarLugar(lugarID) {

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
                url: '../../Lugares/EliminarLugar',
                // la información a enviar
                // (también es posible utilizar una cadena de datos)
                data: { LugarID: lugarID },
                //especifico peticion tipo POST
                type: 'POST',
                //info que se espera de respuesta
                dataType: 'json',
                // código a ejecutar si la petición es satisfactoria;
                // la respuesta es pasada como argumento a la función
                success: function (resultado) {
                    if (!resultado) {
                        Swal.fire({
                            title: "Oops...!",
                            text: "Este registro no se puede eliminar porque ya existe en otra tabla",
                            icon: "error"
                        });
                        ListadoLugares();
                    }
                    else {
                        Swal.fire({
                            title: "¡Hecho!",
                            text: "Su registro ha sido eliminado.",
                            icon: "success"
                        });
                        ListadoLugares();
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

