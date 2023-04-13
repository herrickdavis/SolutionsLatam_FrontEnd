﻿var muestras = function () {
    var filterDataTable;
    var filterDataTableCertificado;
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
        filterDataTableCertificado = new FilterDataTable({
            element: $("#tablaCertificados"),
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
                }
            ],
            setFilterAll: true,
            onFilter: function (filtros, instance) {
                obtenerTablaCertificado(1);
            },
        });
        $('.dropdown-columnas').on('click', function(e) {
            e.stopPropagation();
        });
        agregarAltoDinamicamente($(".table-responsive"), altoTabla);
    }
    var inicializarEvento = function () {
        $(".descargar").on("click", descargarTodo);
        $(".opcionMuestra").on("click", mostrarMuestra);
        $(".opcionCertificado").on("click", mostrarCertificado);
        $(".btnDescargarCertificado").on("click", descargarZipInformes);
        $("#btnRetroceder").on("click", mostrarLista);
        $(".dropdown-columnas input[type=checkbox]").on("change", mostrarOcultarColumnas);
        inicializarEventoMuestra();
        inicializarEventoCertificado();
    }
    var inicializarEventoMuestra = function(){
        $("#footerTableMuestra .my-1").on("click", cambiarPaginaMuestra);
        $("#tablaMuestras tbody tr").on("click", obtenerDetalle);
        $("#tablaMuestras tbody tr td:first-child").on("click", function () { event.stopPropagation(); });
        $("#tablaMuestras tbody tr td:nth-child(2)").on("click", obtenerArchivosMuestras);
        $(".chkDescargaMuestra").on("change", mostrarBotonDescargaMuestra);
        $("#tablaMuestras .chkSeleccionarTodo").on("change", seleccionarTodo);
        $("#divMuestras #ddlPaginado").on("change", obtenerTablaMuestra);
    }
    var inicializarEventoCertificado = function () {
        $("#footerTableCertificado .my-1").on("click", cambiarPaginaCertificado);
        $("#tablaCertificados tbody tr td:nth-child(2)").on("click", obtenerArchivosInforme);
        $(".chkDescargaCertificado").on("change", mostrarBotonDescargaCertificado);
        $("#tablaCertificados .chkSeleccionarTodo").on("change", seleccionarTodoCertificado);
        $("input[type=checkbox][name=chkTipoArchivo]").on("change", cambiarTipoArchivo);
        $("#divCertificados #ddlPaginado").on("change", obtenerTablaCertificado);
    }
    var armarFiltros = function () {
        var filtros = filterDataTable.getFilter();
        return filtros.select(x => { return { cabecera: x.id.toLowerCase(), condicion: x.condition.toLowerCase(), valor: x.value }; })
    }
    var armarFiltrosCertificado = function () {
        var filtros = filterDataTableCertificado.getFilter();
        return filtros.select(x => { return { cabecera: x.id.toLowerCase(), condicion: x.condition.toLowerCase(), valor: x.value }; })
    }
    var cambiarPaginaMuestra = function () {
        var paginaActual = $(this).attr("data-page");
        obtenerTablaMuestra(paginaActual);
    };
    var cambiarPaginaCertificado = function () {
        var paginaActual = $(this).attr("data-page");
        obtenerTablaCertificado(paginaActual);
    };
    var obtenerTablaMuestra = function (page) {
        var rowPage = $("#divMuestras .paginador").val();
        $("#tablaMuestras tbody:first").hide("fast");
        var model = { filtros: armarFiltros() };
        $.post(`${urlMuestras}?page=${page}&rowPage=${rowPage}`, model)
            .then(function (html) {
                $("#divMuestras").empty();
                $("#divMuestras").html(html);
                inicializarEventoMuestra();
                filterDataTable.refresh($("#tablaMuestras"));
                mostrarBotonDescargaMuestra();
                agregarAltoDinamicamente($(".table-responsive"), altoTabla);
            })
            .catch(function (error) {
                $("#tablaMuestras tbody:first").show("fast");
            });
    }
    var obtenerTablaCertificado = function (page) {
        var rowPage = $("#divCertificados .paginador").val();
        $("#tablaCertificados tbody:first").hide("fast");
        var model = { filtros: armarFiltrosCertificado() };
        $.post(`${urlCertificados}?page=${page}&rowPage=${rowPage}`, model)
            .then(function (html) {
                $("#divCertificados").empty();
                $("#divCertificados").html(html);
                inicializarEventoCertificado();
                filterDataTableCertificado.refresh($("#tablaCertificados"));
                mostrarBotonDescargaCertificado();
                agregarAltoDinamicamente($(".table-responsive"), altoTabla);
            })
            .catch(function (error) {
                $("#tablaMuestras tbody:first").show("fast");
            });
    }
    var obtenerDetalle = function () {
        var id = $(this).attr("data-id");
        $.get(`${urlDetalle}/${id}`, ".vistaLista")
            .then(function (html) {
                $("#detalleMuestras").empty();
                $("#detalleMuestras").html(html);
                mostrarDetalle();
                detalleMuestras.init();
            });
    }
    var mostrarDetalle = function () {
        $(".vistaLista").hide("slow");
        $(".vistaDetalle").show("slow");
    }
    var mostrarLista = function () {
        $(".vistaDetalle").hide("slow");
        $(".vistaLista").show("slow");
    }
    var obtenerArchivosMuestras = function () {
        var id = $(this).closest("tr").attr("data-id");
        var model = {
            id_muestra: id
        };
        $.post(urlGetDocumentosMuestra, model)
            .then(function (html) {
                $("#panelBody").empty();
                $("#panelBody").html(html);
                $(".descargarMuestra").on("click", descargarMuestra);
                $(".descargarDocumento").on("click", descargarDocumento);
                kt.show();
            })
            .catch(error => {
                console.log(error);
            });
        event.stopPropagation();
    }
    var obtenerArchivosInforme = function () {
        var tr = $(this).closest("tr");
        var id = tr.attr("data-id");
        descargarArchivo(urlGetDocumentoInformes + "?id=" + id);
    }
    var mostrarBotonDescargaMuestra = function () {
        var chks = $(".chkDescargaMuestra:checked");
        if (chks.length > 0) {
            $(".btnDescargarMuestra").show("fast");
        }
        else {
            $(".btnDescargarMuestra").hide("fast");
        }
    }
    var mostrarBotonDescargaCertificado = function () {
        var chks = $(".chkDescargaCertificado:checked");
        if (chks.length > 0) {
            $(".btnDescargarCertificado").show("fast");
        }
        else {
            $(".btnDescargarCertificado").hide("fast");
        }
    }
    var seleccionarTodo = function () {
        if (this.checked) {
            $(".chkDescargaMuestra").prop("checked", true);
        }
        else {
            $(".chkDescargaMuestra").prop("checked", false);
        }
        mostrarBotonDescargaMuestra();
    }
    var seleccionarTodoCertificado = function () {
        if (this.checked) {
            $(".chkDescargaCertificado").prop("checked", true);
        }
        else {
            $(".chkDescargaCertificado").prop("checked", false);
        }
        mostrarBotonDescargaCertificado();
    }
    var descargarTodo = function () {
        var queryString = "";
        var trs = $(".chkDescargaMuestra:checked").closest("tr");
        $.each(trs, (index, tr) => {
            if (index === 0) {
                queryString = "?id=" + tr.getAttribute("data-id");
            } else {
                queryString += "&id=" + tr.getAttribute("data-id");
            }
        });
        var tipoArchivoSelecciones = $(".checkbox-list input[type=checkbox]:checked");
        $.each(tipoArchivoSelecciones, (index, chk) => {
            queryString += "&id_tipo_archivo=" + chk.getAttribute("data-tipoarchivo");
        });
        $("#modalDescarga").modal("hide");
        descargarArchivo(urlGetZipMuestra + queryString);
    }
    var descargarMuestra = function () {
        var id = $(this).attr("data-id");
        descargarArchivo(urlGetZipMuestra + "?id=" + id);
    }
    var descargarDocumento = function () {
        var id = $(this).closest("li").attr("data-id");
        descargarArchivo(urlGetDocumentoMuestra + "/" + id);
    }
    var descargarZipInformes = function () {
        var queryString = "";
        var trs = $(".chkDescargaCertificado:checked").closest("tr");
        $.each(trs, (index, tr) => {
            if (index === 0) {
                queryString = "?id=" + tr.getAttribute("data-id");
            } else {
                queryString += "&id=" + tr.getAttribute("data-id");
            }
        });
        descargarArchivo(urlGetDocumentoInformes + queryString);
    }
    var mostrarMuestra = function () {
        $("#divMuestras").show("fast");
        mostrarBotonDescargaMuestra();
        $(".btnDescargarCertificado").hide("fast");
        $(".dropdown-btn-columnas").show("fast");
        $("#divCertificados").hide("fast");
    }
    var mostrarCertificado = function () {
        $("#divCertificados").show("fast");
        mostrarBotonDescargaCertificado();
        $(".btnDescargarMuestra").hide("fast");
        $(".dropdown-btn-columnas").hide("fast");
        $("#divMuestras").hide("fast");
    }
    var cambiarTipoArchivo = function () {
        if ($(this).is(":checked")) {
            var tipoArchivo = $(this).attr("data-tipoarchivo");
            if (tipoArchivo === "0") {
                $("input[type=checkbox][name=chkTipoArchivo]").prop("checked", false);
            }
            else {
                $("input[type=checkbox][name=chkTipoArchivo]:last").prop("checked", false);
                
            }
            $(this).prop("checked", true);
        }
        
    }
    var ordenarColumnas  = function(headers){
        var columns = [];
        $.each(headers, (index, value) => {
            if($(value).attr("data-header")){
                columns.push($(value).attr("data-header"));
            }
        })
        var model = {
            numero_tabla: 1,
            orden: columns
        };
        $.post(urlSetColumnasUsuario, model)
         .then(success => {});
    }
    var mostrarOcultarColumnas = function(){
        var isChecked = this.checked;
        var columns = [];
        var checkboxs = $(".dropdown-columnas input[type=checkbox]:checked");
        $.each(checkboxs, (index, value) => {
            if(value.checked && value !== this){
                columns.push(value.getAttribute("data-header"));
            }
        });
        if(isChecked)
            columns.splice(0,0, this.getAttribute("data-header"));

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
