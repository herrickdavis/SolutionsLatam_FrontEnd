var usuario = function () {
    var iniciar = function () {
        inicializarEvento();
        agregarAltoDinamicamente($(".table-responsive"), altoTabla);
        $("#btnSearch").on("click", obtenerTablaUsuario);
    
    }
    var inicializarEvento = function () {
        $("#footerUsuario .my-1").on("click", cambiarPagina);
        $("#ddlPaginado").on("change", obtenerTablaUsuario);
        $(".btnActivarDesactivar").on("click", activarDesactivarUsuario);
    }
    var cambiarPagina = function () {
        var paginaActual = $(this).attr("data-page");
        obtenerTablaUsuario(paginaActual);
    };
    var obtenerTablaUsuario = function(page){
        var rowPage = $("#ddlPaginado").val();
        $(".table tbody:first").hide("fast");
        var model = { nombre: $("#nombre").val(), email: $("#email").val() };
        $.post(`${urlFiltrar}?page=${page}&rowPage=${rowPage}`, model)
            .then(function (html) {
                $("#divUsuario").empty();
                $("#divUsuario").html(html);
                inicializarEvento();
                agregarAltoDinamicamente($(".table-responsive"), altoTabla);
            });
    }
    var activarDesactivarUsuario = function(){
        var id = $(this).attr("data-id");
        $.post(`${urlActivarDesactivarUsuario}/${id}`)
        .then(function (response) {
            if(response.success){
                var page = $(".btn-hover-primary.active").attr("data-page");
                obtenerTablaUsuario(page);
                return toastr.success(response.mensaje);
            }
            else return toastr.error(response.mensaje);
        });
    }
    return {
        init: iniciar
    };
}();
jQuery(document).ready(function () {
    usuario.init();
});
