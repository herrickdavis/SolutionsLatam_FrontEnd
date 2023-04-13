var editarUsuario = function () {
    var iniciar = function () {
        inicializarEvento();
    }
    var inicializarEvento = function () {
        $("#btnEditar").on("click", guardar);
        $(".select2").select2({
            placeholder: placeholder,
        });
        $(".btn-generar").on("click", generarContrasena);
    }
    var guardar = function(){
        event.preventDefault();
        var form = $("form").get(0);
        var model = { 
            nombre: $("#nombre").val(), 
            email: $("#email").val(),
            password: $("#password").val() == "" ? null: $("#password").val(),
            id_rol: $("#id_rol").val(),
            id_empresas: $("#id_empresas").val(),
            idioma: $("#idioma").val(),
            data_campo: $("#data_campo").val(),
            ver_empresa_sol: $("#ver_empresa_sol").is(":checked") ? "S" : "N",
            ver_contacto_sol: $("#ver_contacto_sol").is(":checked") ? "S" : "N",
            ver_empresa_con: $("#ver_empresa_con").is(":checked") ? "S" : "N",
            ver_contacto_con: $("#ver_contacto_con").is(":checked") ? "S" : "N",
        };
        var url = form.action;
        $.post(url, model)
            .then(function (response) {
                if (response.success !== null)
                 return toastr.success(response.mensaje);
                toastr.error(response.mensaje);
            });
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
    editarUsuario.init();
});
