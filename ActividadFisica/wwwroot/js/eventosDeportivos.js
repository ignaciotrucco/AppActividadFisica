window.onload = ListadoEventos();

function ListadoEventos() {

    $.ajax({
        url: '../../EventosDeportivos/ListadoEventos',
        data: { },
        type: 'GET',
        dataType: 'json',
        success: function (listadoEventos) {

            $("#modalEventos").modal("hide");
            LimpiarForm();

            let contenidoTabla = ``;

            $.each(listadoEventos, function (index, evento) {
                if (evento.eliminado) {
                    contenidoTabla += `
                    <tr class="bg-danger">
                        <td><del>${evento.nombre}</del></td>
                        <td></td>
                        <td class="text-center"><button type="button" class="btn btn-danger" onclick="HabilitarEvento(${evento.eventoDeportivoID})">Habilitar</button></td>
                    </tr>
                `;
                }
                else {
                    contenidoTabla += `
                <tr>
                    <td>${evento.nombre}</td>
                    <td class="text-center"><button type="button" class="btn btn-success" onclick="ModalEditar(${evento.eventoDeportivoID})"><i class="fa-solid fa-pen-to-square"></i></button></td>
                    <td class="text-center"><button type="button" class="btn btn-danger" onclick="DeshabilitarEvento(${evento.eventoDeportivoID})">Deshabilitar</button></td>
                </tr>
                `;
                }

            });
            document.getElementById("tbody-eventos").innerHTML = contenidoTabla;
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

function NuevoEvento() {
    $("#tituloModal").text("Nuevo Evento Deportivo")
}

function LimpiarForm() {
    $("#EventoID").val(0);
    $("#eventoNombre").val("");
}

function GuardarEvento() {

    let eventoID = $("#EventoID").val();
    let eventoNombre = $("#eventoNombre").val();

    $.ajax({
        url: '../../EventosDeportivos/GuardarEvento',
        data: {EventoID: eventoID, EventoNombre: eventoNombre },
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {

            if (resultado != "") {
                alert(resultado);
            }
            ListadoEventos();
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

function DeshabilitarEvento(eventoID) {

    $.ajax({
        url: '../../EventosDeportivos/DeshabilitarEvento',
        data: {EventoID: eventoID },
        type: 'POST',
        dataType: 'json',
        success: function (Eliminado) {
            ListadoEventos();
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

function HabilitarEvento(eventoID) {
    $.ajax({
        url: '../../EventosDeportivos/HabilitarEvento',
        data: {EventoID: eventoID },
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            ListadoEventos();
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

function ModalEditar(eventoID) {
    $("#tituloModal").text("Editar Evento Deportivo")
    
    $.ajax({
        url: '../../EventosDeportivos/ListadoEventos',
        data: {EventoID: eventoID },
        type: 'GET',
        dataType: 'json',
        success: function (listadoEventos) {
            let eventoListar = listadoEventos[0]
            $("#EventoID").val(eventoID);
            $("#eventoNombre").val(eventoListar.nombre);
            $("#modalEventos").modal("show");
            
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