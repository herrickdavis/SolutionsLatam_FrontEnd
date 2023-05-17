var muestras = function () {
    var filterDataTable;
    var filterDataTableEstacion;
    var filterDataTableProyecto;
    var kt = new KTOffcanvas("kt_demo_panel", {
        baseClass: "offcanvas",
        overlay: true,
        closeBy: 'kt_aside_close_btn',
        toggleBy: {
            target: 'kt_aside_mobile_toggle',
            state: 'mobile-toggle-active'
        }
    });
    var iniciar = function () {
        inicializarEvento();
        filterDataTable = new FilterDataTable({
            element: $("#tablaMuestras"),
            headerFilter: [
                {
                    headerIndex: 0,
                    filter: false
                },
                {
                    headerIndex: 1,
                    filter: false
                }, {
                    headerIndex: "Fecha Muestreo",
                    type: "datetime"
                },
                {
                    headerIndex: "Estado",
                    type: "select",
                    values: [{
                        text: opcionRecibida,
                        value: "recibida"
                    }, {
                        text: opcionProceso,
                        value: "en proceso"
                    }, {
                        text: opcionFinalizada,
                        value: "finalizada"
                    }, {
                        text: opcionConInforme,
                        value: "con informe"
                    }]
                }
            ],
            setFilterAll: true,
            reColorder: true,
            onFilter: function (filtros, instance) {
                obtenerTablaMuestra(1);
            },
            onAfterSort: ordenarColumnas
        });
        filterDataTableEstacion = new FilterDataTable({
            element: $("#tablaEstaciones"),
            headerFilter: [
                {
                    headerIndex: 0,
                    filter: false
                },
                {
                    headerIndex: 1,
                    filter: false
                }, {
                    headerIndex: "Fecha Muestreo",
                    type: "datetime"
                },
                {
                    headerIndex: "Estado",
                    type: "select",
                    values: [{
                        text: opcionRecibida,
                        value: "recibida"
                    }, {
                        text: opcionProceso,
                        value: "en proceso"
                    }, {
                        text: opcionFinalizada,
                        value: "finalizada"
                    }, {
                        text: opcionConInforme,
                        value: "con informe"
                    }]
                }
            ],
            setFilterAll: true,
            reColorder: true,
            onFilter: function (filtros, instance) {
                obtenerTablaEstacion(1);
            },
            onAfterSort: ordenarColumnas
        });
        filterDataTableProyecto = new FilterDataTable({
            element: $("#tablaProyectos"),
            headerFilter: [
                {
                    headerIndex: 0,
                    type: "text",
                    filter: false
                },
                {
                    headerIndex: 1,
                    type: "text",
                    filter: false
                },
                {
                    headerIndex: 3,
                    type: "text",
                    filter: false
                }
            ],
            setFilterAll: true,
            onFilter: function (filtros, instance) {
                obtenerTablaProyecto(1);
            },
        });
        $('.dropdown-columnas').on('click', function (e) {
            e.stopPropagation();
        });
        agregarAltoDinamicamente($(".table-responsive"), altoTabla);
    }
    var inicializarEvento = function () {

        $(".asignar").on("click", asignarAlias);
        $(".asignarProyecto").on("click", asignarAliasProyecto);
        $(".opcionEstacion").on("click", mostrarEstacion);
        $(".opcionProyecto").on("click", mostrarProyecto);
        $(".btnDescargarCOC").on("click", obtenerArchivosCOC);
        $("#btnRetroceder").on("click", mostrarLista);
        $(".dropdown-columnas input[type=checkbox]").on("change", mostrarOcultarColumnas);
        //inicializarEventoMuestra();
        inicializarEventoEstaciones();
    }

    var inicializarEventoEstaciones = function () {
        $("#footerTableEstacion .my-1").on("click", cambiarPaginaEstacion);
        $("#footerTableProyectos .my-1").on("click", cambiarPaginaProyecto);
        $("#tablaEstaciones tbody tr td:first-child").on("click", function () { event.stopPropagation(); });
        $("#tablaProyectos tbody tr td:first-child").on("click", function () { event.stopPropagation(); });
        //$(".chkDescargaMuestra").on("change", mostrarBotonDescargaMuestra);
        //$(".chkDescargaProyecto").on("change", mostrarBotonDescargaProyecto);
        //$("#tablaMuestras .chkSeleccionarTodo").on("change", seleccionarTodo);
        //$("#divMuestras #ddlPaginado").on("change", obtenerTablaMuestra);
    }
    var armarFiltros = function () {
        var filtros = filterDataTable.getFilter();
        return filtros.select(x => { return { cabecera: x.id.toLowerCase(), condicion: x.condition.toLowerCase(), valor: x.value }; })
    }
    var armarFiltrosEstacion = function () {
        var filtros = filterDataTableEstacion.getFilter();
        return filtros.select(x => { return { cabecera: x.id.toLowerCase(), condicion: x.condition.toLowerCase(), valor: x.value }; })
    }

    var armarFiltrosProyecto = function () {
        var filtros = filterDataTableProyecto.getFilter();
        return filtros.select(x => { return { cabecera: x.id.toLowerCase(), condicion: x.condition.toLowerCase(), valor: x.value }; })
    }

    var cambiarPaginaProyecto = function () {
        var paginaActual = $(this).attr("data-page");
        obtenerTablaProyecto(paginaActual);
    };
    var cambiarPaginaEstacion = function () {
        var paginaActual = $(this).attr("data-page");
        console.log(paginaActual)
        obtenerTablaEstacion(paginaActual);
    };

    var obtenerTablaProyecto = function (page) {
        var rowPage = $("#divProyectos .paginador").val();
        $("#tablaProyectos tbody:first").hide("fast");
        var model = { filtros: armarFiltrosProyecto() };
        $.post(`${urlProyectos}?page=${page}&rowPage=${rowPage}`, model)
            .then(function (html) {
                $("#divProyectos").empty();
                $("#divProyectos").html(html);
                inicializarEventoEstaciones();
                filterDataTableProyecto.refresh($("#tablaProyectos"));
                //mostrarBotonDescargaMuestra();
                agregarAltoDinamicamente($(".table-responsive"), altoTabla);
            })
            .catch(function (error) {
                $("#tablaProyectos tbody:first").show("fast");
            });
    }
    var obtenerTablaEstacion = function (page) {
        var rowPage = $("#divEstaciones .paginador").val();
        $("#tablaEstaciones tbody:first").hide("fast");
        var model = { filtros: armarFiltrosEstacion() };
        $.post(`${urlEstaciones}?page=${page}&rowPage=${rowPage}`, model)
            .then(function (html) {
                console.log(urlEstaciones)
                $("#divEstaciones").empty();
                $("#divEstaciones").html(html);
                inicializarEventoEstaciones();
                filterDataTableEstacion.refresh($("#tablaEstaciones"));
                //mostrarBotonDescargaMuestra();
                agregarAltoDinamicamente($(".table-responsive"), altoTabla);
            })
            .catch(function (error) {
                $("#tablaEstaciones tbody:first").show("fast");
            });
    }



    var mostrarLista = function () {
        $(".vistaDetalle").hide("slow");
        $(".vistaLista").show("slow");
    }


    var seleccionarTodo = function () {
        if (this.checked) {
            $(".chkDescargaMuestra").prop("checked", true);
        }
        else {
            $(".chkDescargaMuestra").prop("checked", false);
        }
        //mostrarBotonDescargaMuestra();
    }

    var asignarAlias = function () {
        var id = [];
        var alias = $("#inputAlias").val();;
        var trs = $(".chkDescargaMuestra:checked").closest("tr");
        var page = $(".btn.btn-icon.btn-sm.border-0.btn-hover-primary.active").attr("data-page")
        $.each(trs, (index, tr) => {
            id.push(tr.getAttribute("data-id"))
        });

        console.log(alias)
        $("#modalAsignar").modal("hide");

        var model = {
            id: id,
            alias: alias
        };
        $.post(urlEstacionesAsignar, model)
            .then(function (html) {
                console.log(html)
                obtenerTablaEstacion(page)
            })
            .catch(error => {
                console.log(error);
            });
    }

    var asignarAliasProyecto = function () {
        var id = [];
        var alias = $("#inputAliasProyecto").val();
        var trs = $(".chkDescargaProyecto:checked").closest("tr");
        var page = $("#footerTableProyectos .btn.btn-icon.btn-sm.border-0.btn-hover-primary.active").attr("data-page")
        $.each(trs, (index, tr) => {
            id.push(tr.getAttribute("data-id"))
        });

        $("#modalAsignarProyecto").modal("hide");
        console.log(page)
        console.log(id)
        console.log(alias)
        var model = {
            id: id,
            alias: alias
        };
        $.post(urlProyectosAsignar, model)
            .then(function (html) {
                console.log(html)
                obtenerTablaProyecto(page)
            })
            .catch(error => {
                console.log(error);
            });
    }

    var obtenerArchivosCOC = function () {
        var id = [];        
        var trs = $(".chkDescargaMuestra:checked").closest("tr");
        $.each(trs, (index, tr) => {
            id.push(tr.getAttribute("data-id"))
        });
        var model = {
            id_documento: id
        };
        $.post({
            url: urlGenerarCadena,
            data: model,
            xhrFields: {
                responseType: 'blob'
            },
            success: function (data) {
                var filename = "Archivo.xlsx";
                var blob = new Blob([data], { type: 'application/octet-stream' });

                // Create a temporary link to download the file
                var downloadLink = document.createElement('a');
                downloadLink.href = window.URL.createObjectURL(blob);
                downloadLink.download = filename;
                downloadLink.click();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }

    var mostrarEstacion = function () {
        $("#divEstaciones").show("fast");
        //mostrarBotonDescargaMuestra();
        $(".btnDescargarCertificado").hide("fast");
        $(".dropdown-btn-columnas").show("fast");
        $("#divProyectos").hide("fast");
    }
    var mostrarProyecto = function () {
        $("#divProyectos").show("fast");
        //mostrarBotonDescargaCertificado();
        $(".btnDescargarMuestra").hide("fast");
        $(".dropdown-btn-columnas").hide("fast");
        $("#divEstaciones").hide("fast");
    }

    var ordenarColumnas = function (headers) {
        var columns = [];
        $.each(headers, (index, value) => {
            if ($(value).attr("data-header")) {
                columns.push($(value).attr("data-header"));
            }
        })
        var model = {
            numero_tabla: 1,
            orden: columns
        };
        $.post(urlSetColumnasUsuario, model)
            .then(success => { });
    }
    var mostrarOcultarColumnas = function () {
        var isChecked = this.checked;
        var columns = [];
        var checkboxs = $(".dropdown-columnas input[type=checkbox]:checked");
        $.each(checkboxs, (index, value) => {
            if (value.checked && value !== this) {
                columns.push(value.getAttribute("data-header"));
            }
        });
        if (isChecked)
            columns.splice(0, 0, this.getAttribute("data-header"));

        var model = {
            numero_tabla: 1,
            orden: columns
        };
        $.post(urlSetColumnasUsuario, model)
            .then(success => obtenerTablaMuestra(1));
    }
    return {
        init: iniciar,
        refresh: function () {
            obtenerTablaMuestra();
            obtenerTablaCertificado();
        }
    };
}();
jQuery(document).ready(function () {
    muestras.init();
});
