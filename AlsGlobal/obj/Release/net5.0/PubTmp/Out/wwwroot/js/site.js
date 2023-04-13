var oldGet = $.get;
var oldPost = $.post;
var elementLoad = $("body");

$.get = function (argument, query) {
    elementLoad = query === undefined ? $("body") : $(query);
    return oldGet(argument);
};
$.post = function (argument, data, query) {
    data = data || $("body");
    query = query || $("body");
    elementLoad = typeof data === "object" ? query : data;
    return oldPost(argument, data);
};
$(document)
    .ajaxStart(function (ajax) {
        KTApp.block(elementLoad, {});
    })
    .ajaxError(function myErrorHandler(event, xhr, ajaxOptions, thrownError) {
        debugger;
        if (xhr.status === 401) {
            document.location.href = urlLogin;
        }
    })
    .ajaxStop(function () {
        KTApp.unblock(elementLoad);
    });

var removerBotonAllSelect = function () {
    $(".actions-btn.bs-select-all.btn.btn-light").remove();
}
var cambiarIdiomaSelect = function () {
    $(".actions-btn.bs-deselect-all.btn.btn-light").html(select_Deseleccionar);
}
jQuery(document).ready(function () {
    setTimeout(function () {
        removerBotonAllSelect();
        cambiarIdiomaSelect();
    }, 1000);
});

var descargarArchivo = function (url, method = 'get', model = {}) {
    $.ajax({
        type: method,
        url: url,
        data: model,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (response, status, xhr) {
            debugger;
            if (response.message !== undefined) {
                toastr.error(error);
            }
            else {
                var filename = obtenerNombreArchivo(xhr);
                var blob = new Blob([response], { type: response.type });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.setAttribute("download", filename);
                link.click();
            }
        },
        catch: function () {
            toastr.error(error);
        },
        error: function (response) {
            toastr.error(error);
        }
    });
}
var obtenerNombreArchivo = function (xhr) {
    var filename = "";
    var disposition = xhr.getResponseHeader('Content-Disposition');
    if (disposition && disposition.indexOf('attachment') !== -1) {
        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
        var matches = filenameRegex.exec(disposition);
        if (matches != null && matches[1]) {
            filename = matches[1].replace(/['"]/g, '');
        }
    }
    return filename;
}
var agregarAltoDinamicamente = function (element, porcentage = 1) {
    var altoWindow = parseInt(window.innerHeight);
    var altoDinamico = parseInt(altoWindow) * porcentage;
    $(element).css({ "max-height": `${altoDinamico}px`, 'overflow-y': 'auto' });
}
var cambiarEmpresa = function () {
    var empresaSeleccionada = $("#kt_quick_user select").val();
    if (empresaSeleccionada === "") toastr.error(errorSeleccionarEmpresa);

    $.post(urlCambiarEmpresa + "/" + empresaSeleccionada, {}, $("#kt_quick_user"))
        .then(success => {
            if (success.successBoolean) {
                document.location.reload();
            }
            else toastr.error(success.mensaje);
        });
   
}
$("#kt_quick_user .btnCambiar").on("click", cambiarEmpresa);