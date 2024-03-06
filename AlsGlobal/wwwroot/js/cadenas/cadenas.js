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
        obtenerEmpresas();
        $(".btnDescargarCOC").on("click", obtenerArchivosCOC);
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
        $("#btnRetroceder").on("click", mostrarLista);
        $(".dropdown-columnas input[type=checkbox]").on("change", mostrarOcultarColumnas);
        $(".editar_plantilla").on("click", mostralModelEditar);
        $("#editarPlantilla").on("click", editarPlantilla);
        $(".btnEliminarPlantilla").on("click", verEliminarPlantilla);
        $(".btnConfirmarEliminacion").on("click", eliminarPlantilla);
        $("#regiones").on("change", obtenerEmpresas);
        $("#SeleccionEmpresa").on('click', mostrarCadenas)
        $("#tablaEstaciones .chkSeleccionarTodo").on("change", seleccionarTodo);
        inicializarEventoEstaciones();
    }

    var inicializarEventoEstaciones = function () {
        $("#footerTableEstacion .my-1").on("click", cambiarPaginaEstacion);
        $("#footerTableProyectos .my-1").on("click", cambiarPaginaProyecto);
        $("#tablaEstaciones tbody tr td:first-child").on("click", function () { event.stopPropagation(); });
        $("#tablaProyectos tbody tr td:first-child").on("click", function () { event.stopPropagation(); });
    }

    var mostrarCadenas = function () {
        var paginaActual = 1
        obtenerTablaEstacion(paginaActual);
    }

    var obtenerEmpresas = function () {
        var id_pais = $("#regiones").val();
        var model = {
            id_pais: id_pais,
        }
        $.post(urlObtenerEmpresas, model)
            .then(response => {
                $("#id_empresa").empty();
                response.forEach(item => {
                    console.log()
                    var option = document.createElement("OPTION");
                    option.value = item.value;
                    option.innerText = item.text;
                    $("#id_empresa").append(option);
                });
                $('#id_empresa').selectpicker('refresh');
                
            })
            .catch(error => console.log(error));
    }
    var armarFiltrosEstacion = function () {
        var filtros = filterDataTableEstacion.getFilter();
        return filtros.select(x => { return { cabecera: x.id.toLowerCase(), condicion: x.condition.toLowerCase(), valor: x.value }; })
    }

    var mostralModelEditar = function () {
        $("#nombrePlantilla").val($(this).data("nombre"))
        $("#idPlantilla").val($(this).data("id"))
        $('#modalEditar').modal('show')
    }

    var editarPlantilla = function () {
        var id = $('#idPlantilla').val() 
        var nombre_plantilla = $('#nombrePlantilla').val()
        var file_data = $('#editar_archivo_plantilla').prop('files')[0];
        var form_data = new FormData();
        form_data.append('archivo', file_data);
        form_data.append('nombrePlantilla', nombre_plantilla);
        form_data.append('id', id);

        $.ajax({
            url: urlEditarPlantilla, //aquí debes colocar la URL de tu backend donde procesarás el archivo
            dataType: 'text',
            cache: false,
            contentType: false,
            processData: false,
            data: form_data,
            type: 'post',
            success: function (response) {
                //Aquí puedes manejar la respuesta de tu backend luego de que el archivo haya sido procesado
                console.log(response);
                $('#modalEditar').modal('hide')
                $('#modalPlantilla').modal('hide')
                toastr.success("Se edito correctamente");
                location.reload();
            },
            error: function (response) {
                //Aquí puedes manejar los errores
                console.log(response);
            }
        }); 
    }

    var cambiarPaginaProyecto = function () {
        var paginaActual = $(this).attr("data-page");
        obtenerTablaProyecto(paginaActual);
    };
    var cambiarPaginaEstacion = function () {
        var paginaActual = $(this).attr("data-page");
        obtenerTablaEstacion(paginaActual);
    };

    var obtenerTablaEstacion = function (page) {
        var rowPage = $("#divEstaciones .paginador").val();
        var id_pais = $("#regiones").val();
        var id_empresas = $("#id_empresa").val();
        console.log(id_empresas);
        $("#tablaEstaciones tbody:first").hide("fast");
        var model = {
            filtros: armarFiltrosEstacion(),
            id_pais: id_pais,
            id_empresas: id_empresas
        };
        $.post(`${urlObtenerDataCOC}?page=${page}&rowPage=${rowPage}`, model)
            .then(function (html) {
                $("#divEstaciones").empty();
                $("#divEstaciones").html(html);
                inicializarEvento();
                inicializarEventoEstaciones();
                filterDataTableEstacion.refresh($("#tablaEstaciones"));
                agregarAltoDinamicamente($(".table-responsive"), altoTabla);
                $("#modalEmpresa").modal('hide')
            })
            .catch(function (error) {
                $("#tablaEstaciones tbody:first").show("fast");
                $("#modalEmpresa").modal('hide')
            });
    }

    var verEliminarPlantilla = function () {
        var id = $(this).data("id")
        var nombre = $(this).data("nombre")
        $("#modalPlantilla").modal("hide")
        $(".textoPlantillaEliminar").text(nombre)
        $("#modalVerificar").modal('show')
        $(".btnConfirmarEliminacion").attr("data-id", id);
        console.log(id)
        console.log(nombre)
    }

    var eliminarPlantilla = function () {
        var id = $(this).data("id")
        var model = {
            id: id
        };
        console.log(id)
        $.ajax({
            url: urlEliminarArchivo,
            type: 'DELETE',
            data: model,
            success: function (data, textStatus, jqXHR) {
                toastr.success("Se elimino correctamente");
                $('#modalVerificar').modal('hide')
                location.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }

    var mostrarLista = function () {
        $(".vistaDetalle").hide("slow");
        $(".vistaLista").show("slow");
    }


    var seleccionarTodo = function () {
        console.log("check")
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
        var id_plantilla = $("#id_plantilla").val();
        var trs = $(".chkDescargaMuestra:checked").closest("tr");
        $.each(trs, (index, tr) => {
            id.push(tr.getAttribute("data-id"))
        });

        if (id.length === 0) {
            toastr.error('Seleccionar Muestras');
            return false;
        }

        var model = {
            id: id,
            id_plantilla: id_plantilla
        };
        $.post({
            url: urlGenerarCadena,
            data: model, 
            xhrFields: {
                responseType: 'blob'
            },
            success: function (data) {
                var date = new Date();
                var day = date.getDate().toString().padStart(2, '0');
                var month = (date.getMonth() + 1).toString().padStart(2, '0'); // +1 porque getMonth() retorna 0-11
                var year = date.getFullYear();
                var filename = "COC_" + day + "_" + month + "_" + year + ".xlsx";
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

function descargarPlantilla(id) {
    var model = {
        id: id
    };
    $.post({
        url: urlDescargarPlantilla,
        data: model,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data, textStatus, jqXHR) {
            var filename = "";
            var disposition = jqXHR.getResponseHeader('Content-Disposition');

            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);

                if (matches !== null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }

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

function cargarPlantilla() {
    var nombre_plantilla = $('#nombre_plantilla').val()
    var file_data = $('#archivo_plantilla').prop('files')[0];
    var form_data = new FormData();
    form_data.append('archivo', file_data);
    form_data.append('nombrePlantilla', nombre_plantilla);

    $.ajax({
        url: urlCargarPlantilla, //aquí debes colocar la URL de tu backend donde procesarás el archivo
        dataType: 'text',
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: 'post',
        success: function (response) {
            //Aquí puedes manejar la respuesta de tu backend luego de que el archivo haya sido procesado
            console.log(response);
            location.reload();
        },
        error: function (response) {
            //Aquí puedes manejar los errores
            console.log(response);
        }
    });
}
