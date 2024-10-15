window.onload = ListadoEjerciciosFisicos();

// function ListadoEjerciciosFisicos() {
//     $.ajax({
//         url: '../../EjerciciosFisicos/ListadoEjerciciosFisicos',
//         data: { },
//         type: 'POST',
//         dataType: 'json',
//         success: function (ejerciciosFisicoMostrar) {

//             $("#ModalEjerciciosFisicos").modal("hide");
//             LimpiarModal();

//             let contenidoTabla = ``;

//             $.each(ejerciciosFisicoMostrar, function (index, ejerciciosFisicoMostrar) {
//                 contenidoTabla += `
//                 <tr>
//                     <td>${ejerciciosFisicoMostrar.tipoEjercicioDescripcion}</td>
//                     <td>${ejerciciosFisicoMostrar.fechaInicioString}</td>
//                     <td>${ejerciciosFisicoMostrar.fechaFinString}</td>
//                     <td>${ejerciciosFisicoMostrar.estadoEmocionalInicio}</td>
//                     <td>${ejerciciosFisicoMostrar.estadoEmocionalFin}</td>
//                     <td>${ejerciciosFisicoMostrar.observaciones}</td>
//                     <td class="text-center"><button type="button" class="btn btn-success" onclick="AbrirModalEditar(${ejerciciosFisicoMostrar.ejercicioFisicoID})"><i class="fa-solid fa-pen-to-square"></i></button></td>
//                     <td class="text-center"><button type="button" class="btn btn-danger" onclick="EliminarEjercicio(${ejerciciosFisicoMostrar.ejercicioFisicoID})"><i class="fa-solid fa-trash"></i></button></td>
//                 </tr>
//                 `;
//             });
//             document.getElementById("tbody-ejerciciosfisicos").innerHTML = contenidoTabla;
//         },

//         error: function (xhr, status) {
//             Swal.fire({
//                 icon: "error",
//                 title: "Oops...",
//                 text: "Algo salió mal",
//             });
//         }
//     });
// }
function LimpiarFiltros() {
    $("#TipoEjercicioBuscarID").val(0);
    $("#FechaDesde").val("");
    $("#FechaHasta").val("");
}

function ListadoEjerciciosFisicos() {
    let fechadesde = document.getElementById("FechaDesde").value;
    let fechahasta = document.getElementById("FechaHasta").value;
    let tipoEjercicioBuscar = document.getElementById("TipoEjercicioBuscarID").value;
    $.ajax({
        url: '../../EjerciciosFisicos/ListadoEjerciciosFisicos',
        data: {
            FechaDesde: fechadesde,
            FechaHasta: fechahasta,
            TipoEjercicioBuscar: tipoEjercicioBuscar
        },
        type: 'POST',
        dataType: 'json',
        success: function (ejerciciosFisicoMostrar) {

            $("#ModalEjerciciosFisicos").modal("hide");
            LimpiarModal();

            let contenidoTabla = ``;

            $.each(ejerciciosFisicoMostrar, function (index, ejerciciosFisicoMostrar) {
                contenidoTabla += `
                <tr>
                    <td class="text-center">${ejerciciosFisicoMostrar.tipoEjercicioDescripcion}</td>
                    <td class="text-center">${ejerciciosFisicoMostrar.lugarNombre}</td>
                    <td class="text-center">${ejerciciosFisicoMostrar.eventoDeportivoNombre}</td>
                    <td class="text-center">${ejerciciosFisicoMostrar.fechaInicioString}</td>
                    <td class="text-center">${ejerciciosFisicoMostrar.estadoEmocionalInicio}</td>
                    <td class="text-center">${ejerciciosFisicoMostrar.fechaFinString}</td>
                    <td class="text-center">${ejerciciosFisicoMostrar.estadoEmocionalFin}</td>
                    <td>${ejerciciosFisicoMostrar.observaciones}</td>
                    <td class="text-center"><button type="button" class="btn btn-success" onclick="AbrirModalEditar(${ejerciciosFisicoMostrar.ejercicioFisicoID})"><i class="fa-solid fa-pen-to-square"></i></button></td>
                    <td class="text-center"><button type="button" class="btn btn-danger" onclick="EliminarEjercicio(${ejerciciosFisicoMostrar.ejercicioFisicoID})"><i class="fa-solid fa-trash"></i></button></td>
                </tr>
                `;
            });
            document.getElementById("tbody-ejerciciosfisicos").innerHTML = contenidoTabla;
        },

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
    document.getElementById("TipoEjercicioID").value = 0;
    document.getElementById("LugarID").value = 0;
    document.getElementById("EventoID").value = 0;
    document.getElementById("fechainicio").value = null;
    document.getElementById("fechafin").value = null;
    document.getElementById("EstadoEmocionalInicio").value = 0;
    document.getElementById("EstadoEmocionalFin").value = 0;
    document.getElementById("observaciones").value = "";
}

function NuevoRegistro() {
    $("#tituloModal").text("Nuevo Ejercicio Fisico")
}

function AbrirModalEditar(ejercicioFisicosID) {
    $.ajax({
        url: '../../EjerciciosFisicos/LlamarDatosAlModal',
        data: { ejercicioFisicoID: ejercicioFisicosID },
        type: 'POST',
        datatype: 'json',
        success: function (ejerciciosFisicos) {
            let ejercicioFisico = ejerciciosFisicos[0];

            document.getElementById("IdEjercicios").value = ejercicioFisicosID;
            $("#tituloModal").text("Editar ejercicio fisico")
            document.getElementById("TipoEjercicioID").value = ejercicioFisico.tipoEjercicioID;
            document.getElementById("LugarID").value = ejercicioFisico.lugarID;
            document.getElementById("EventoID").value = ejercicioFisico.eventoDeportivoID;
            document.getElementById("fechainicio").value = ejercicioFisico.inicio;
            document.getElementById("fechafin").value = ejercicioFisico.fin;
            document.getElementById("EstadoEmocionalInicio").value = ejercicioFisico.estadoEmocionalInicio;
            document.getElementById("EstadoEmocionalFin").value = ejercicioFisico.estadoEmocionalFin;
            document.getElementById("observaciones").value = ejercicioFisico.observaciones;

            $("#ModalEjerciciosFisicos").modal("show");

        },
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
    //GUARDO EN UNA VARIABLE LOS DATOS DE LOS INPUTS

    let ejercicioFisicoID = document.getElementById("IdEjercicios").value;
    let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    let lugarID = document.getElementById("LugarID").value;
    let eventoID = document.getElementById("EventoID").value;
    let inicio = document.getElementById("fechainicio").value;
    let fin = document.getElementById("fechafin").value;
    let estadoEmocionalInicio = document.getElementById("EstadoEmocionalInicio").value;
    let estadoEmocionalFin = document.getElementById("EstadoEmocionalFin").value;
    let observaciones = document.getElementById("observaciones").value;

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
                url: '../../EjerciciosFisicos/GuardarEjerciciosFisicos',
                // la información a enviar
                // (también es posible utilizar una cadena de datos)
                data: {
                    ejercicioFisicoID: ejercicioFisicoID,
                    tipoEjercicioID: tipoEjercicioID,
                    LugarID: lugarID,
                    inicio: inicio,
                    fin: fin,
                    estadoEmocionalInicio: estadoEmocionalInicio,
                    estadoEmocionalFin: estadoEmocionalFin,
                    observaciones: observaciones,
                    EventoID: eventoID
                },
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
                    ListadoEjerciciosFisicos();
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
            ListadoEjerciciosFisicos();
        }
    });



}



function EliminarEjercicio(ejercicioFisicoID) {
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
                url: '../../EjerciciosFisicos/EliminarEjercicio',
                // la información a enviar
                // (también es posible utilizar una cadena de datos)
                data: { ejercicioFisicoID: ejercicioFisicoID },
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
                        ListadoEjerciciosFisicos();
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

