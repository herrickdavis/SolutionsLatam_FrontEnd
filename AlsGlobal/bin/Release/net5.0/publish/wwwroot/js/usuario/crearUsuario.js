var crearUsuario = function () {
    var iniciar = function () {
        inicializarEvento();
    }
    var inicializarEvento = function () {
        $("#btnCrear").on("click", guardar);
        $(".select2").select2({
            placeholder: placeholder,
        });
        $(".btn-generar").on("click", generarContrasena);
    }
    var guardar = function(){
        var model = { 
            nombre: $("#nombre").val(), 
            email: $("#email").val(),
            password: $("#password").val(),
            id_rol: $("#id_rol").val(),
            id_empresas: $("#id_empresas").val(),
            idioma: $("#idioma").val(),
            data_campo: $("#data_campo").val(),
            ver_empresa_solicitante: $("#ver_empresa_solicitante").is(":checked") ? "S" : "N",
            ver_contacto_solicitante: $("#ver_contacto_solicitante").is(":checked") ? "S" : "N",
            ver_empresa_contratante: $("#ver_empresa_contratante").is(":checked") ? "S" : "N",
            ver_contacto_contratante: $("#ver_contacto_contratante").is(":checked") ? "S" : "N",
        };
        $.post(`${urlCrear}`, model)
            .then(function (response) {
                if (response.success !== null){
                    limpiar();
                    return toastr.success(response.mensaje);
                }
                toastr.error(response.mensaje);
            });
    }
    var limpiar = function(){
        $("#nombre").val("");
        $("#email").val("");
        $("#password").val("");
        $("#id_rol").val("");
        $("#id_empresas").val("");
        $("#idioma").val("");
        $("#data_campo").val("");
        $("#ver_empresa_solicitante").prop("checked", false);
        $("#ver_contacto_solicitante").prop("checked", false);
        $("#ver_empresa_contratante").prop("checked", false);
        $("#ver_contacto_contratante").prop("checked", false);
    }
    var generarContrasena = function(){
        $.get(urlGenerarContrasena)
            .then(function (response) {
                $("#password").val(response);
            });
    }
    return {
        init: iniciar
    };
}();
jQuery(document).ready(function () {
    crearUsuario.init();
});
