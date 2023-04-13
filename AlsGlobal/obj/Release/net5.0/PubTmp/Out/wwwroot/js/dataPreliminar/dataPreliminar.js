var dataPreliminar = function () {
    var filterDataTable;
    var iniciar = function () {
        inicializarEvento();
        filterDataTable = new FilterDataTable({
            element: $("table"),
            headerFilter: [
                {
                    headerIndex: 0,
                    type: "text",
                    filter: true
                },
                {
                    headerIndex: 1,
                    type: "text",
                    filter: true
                },
                {
                    headerIndex: 2,
                    type: "text",
                    filter: true
                },
                {
                    headerIndex: 3,
                    type: "text",
                    filter: true
                },
                {
                    headerIndex: 4,
                    type: "datetime",
                    filter: true
                }
            ],
            onFilter: function (filtros, instance) {
                obtenerTabla(1);
            },
        });

        agregarAltoDinamicamente($(".table-responsive"), altoTabla);
        $("#ddlPaginado").on("change", obtenerTabla);
    }
    var inicializarEvento = function () {
        $("#footerDataPreliminar .my-1").on("click", cambiarPagina);
    }
    var armarFiltros = function () {
        var filtros = filterDataTable.getFilter();
        return filtros.select(x => { return { cabecera: x.id.toLowerCase(), condicion: x.condition.toLowerCase(), valor: x.value }; })
    }
    var cambiarPagina = function () {
        var paginaActual = $(this).attr("data-page");
        obtenerTabla(paginaActual);
    };
    var obtenerTabla = function (page) {
        var rowPage = $("#ddlPaginado").val();
        $(".table tbody:first").hide("fast");
        var model = { filtros: armarFiltros() };
        $.post(`${urlDataPreliminar}?page=${page}&rowPage=${rowPage}`, model)
            .then(function (html) {
                $(".card-body").empty();
                $(".card-body").html(html);
                inicializarEvento();
                filterDataTable.refresh($("table"));
                agregarAltoDinamicamente($(".table-responsive"), altoTabla);
            });
    }
    return {
        init: iniciar
    };
}();
jQuery(document).ready(function () {
    dataPreliminar.init();
});
