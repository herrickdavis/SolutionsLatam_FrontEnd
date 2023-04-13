var perfil = function () {
    var iniciar = function () {
        inicializarEvento();
    }
    var inicializarEvento = function () {
        $("#btnGuardar").on("click", guardar);
    }
    var guardar = function(){
        var model = {
            password: $("#password").val(),
            nueva_clave: $("#nueva_clave").val(),
        }
        $.post(urlCambiarPassword, model)
            .then(response => {
                if (response.success)
                    toastr.success(response.message);
                else
                    toastr.error(response.message);
                
            })
    }
    return {
        init: iniciar
    };
}();
jQuery(document).ready(function () {
    perfil.init();
});
