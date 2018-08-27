
function DesactivarDecimoCuarto(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = false;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoDecimoCuarto',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {
        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}


function ActivarDecimoCuarto(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = true;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoDecimoCuarto',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {
        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}

function DesactivarDecimoTercero(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = false;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoDecimoTercero',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {
        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}

function ActivarDecimoTercero(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = true;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoDecimoTercero',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {
        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}







function ActivarDerechoFondoReserva(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = true;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoDerechoFondosReservas',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {

        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}

function DesactivarDerechoFondoReserva(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = false;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoDerechoFondosReservas',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {

        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}

function ModalidadIESSFondosReservas(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = false;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoFondosReservas',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {

        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}



function ModalidadCuentaFondosReservas(idEmpleado) {
    mostrarLoadingPanel("content", "")
    var estado = true;

    $.ajax({
        type: 'POST',
        url: '../../Empleados/CambiarEstadoFondosReservas',
        dataType: 'json',
        data: { idEmpleado: idEmpleado, estado: estado },
        success: function (data) {

        }, complete: function (data) {
            if (data = true) {
                mostrarNotificacion("Satisfactorio", "El registro se ha actualizado satisfactoriamente")
            }
            else {
                mostrarNotificacion("Error", "Ha ocurrido un error al actualizar el registro")
            }
            $("#content").waitMe("hide");
        },

        error: function (ex) {
            mostrarNotificacion("Error", "Ha ocurrido un error al al conectarse con el servicio...");
        }
    });
}