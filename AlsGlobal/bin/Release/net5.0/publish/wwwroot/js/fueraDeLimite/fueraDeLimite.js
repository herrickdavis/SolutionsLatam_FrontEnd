var fueraDeLimite = function () {
    var iniciar = function () {
        inicializarEvento();
    }
    var inicializarEvento = function () {
        $('#rangoFechas').daterangepicker({
            buttonClasses: ' btn',
            applyClass: 'btn-primary',
            cancelClass: 'btn-secondary',
            //startDate: new Date("2019-12-31"),
            locale: {
                format: 'YYYY/MM/DD'
            }
        });
        $("#btnBuscar").on("click", obtenerReporte);
        $("#rangoFechas").on('apply.daterangepicker', obtenerTipoMuestra);
        $("#id_tipo_muestra").on('change', obtenerEstacion);
        $(".btn-group .btn.btn-success").on("click", descargarData);
    }
    var obtenerTipoMuestra = function (ev, picker) {
        var fecha_inicio = picker.startDate;
        var fecha_fin = picker.endDate;
        $.get(urlGetTipoMuestras + `?fecha_inicio=${fecha_inicio.format('MM-DD-YYYY')}&fecha_fin=${fecha_fin.format('MM-DD-YYYY')}`, ".card-body:first")
            .then(response => {
                $("select").empty();
                response.forEach(x => {
                    var option = document.createElement("OPTION");
                    option.value = x.value;
                    option.innerText = x.text;
                    $("#id_tipo_muestra").append(option);
                });
                $('select').selectpicker('refresh');
            });
    }
    var obtenerEstacion = function (ev) {
        var fecha_inicio = $('#rangoFechas').data('daterangepicker').startDate;
        var fecha_fin = $('#rangoFechas').data('daterangepicker').endDate;
        $.get(urlObtenerEstaciones + `?fecha_inicio=${fecha_inicio.format('MM-DD-YYYY')}&fecha_fin=${fecha_fin.format('MM-DD-YYYY')}&id_tipo_muestra=${this.value}`, "#divEstacion")
            .then(response => {
                $("#id_estacion").empty();
                response.forEach(x => {
                    var option = document.createElement("OPTION");
                    option.value = x.value;
                    option.innerText = x.text;
                    $("#id_estacion").append(option);
                });
                $('#id_estacion').selectpicker('refresh');
            });
    }
    var obtenerReporte = function () {
        var fecha_inicio = $('#rangoFechas').data('daterangepicker').startDate;
        var fecha_fin = $('#rangoFechas').data('daterangepicker').endDate;
        var model = {
            fecha_inicio: fecha_inicio.format('YYYY-MM-DD'),
            fecha_fin: fecha_fin.format('YYYY-MM-DD'),
            id_tipo_muestra: $("#id_tipo_muestra").val(),
            id_estaciones: $("#id_estacion").val()
        };
        $.post(urlGetReporteFueraLimites, model, ".card.card-custom.gutter-b")
            .then(response => {
                $("#divDetalle").empty();
                $("#divDetalle").html(response);
                mostrarMapa();
            });
    }
    var descargarData = function () {
        var fecha_inicio = $('#rangoFechas').data('daterangepicker').startDate;
        var fecha_fin = $('#rangoFechas').data('daterangepicker').endDate;
        var model = {
            fecha_inicio: fecha_inicio.format('YYYY-MM-DD'),
            fecha_fin: fecha_fin.format('YYYY-MM-DD')
        }

        descargarArchivo(urlGetReporteFueraLimiteExcel, 'post', model);
    }
    var mostrarMapa = function () {
        var divsMapa = $(".divMapa");
        $.each(divsMapa, (index, item) => {
            var map = new GMaps({
                div: '#' + item.getAttribute("id"),
                lat: item.getAttribute("data-latitud"),//-12.04318,
                lng: item.getAttribute("data-longitud"),//-77.02824,//
                zoomControl: false,
                mapTypeControl: false,
                scaleControl: false,
                streetViewControl: false,
                rotateControl: false,
                fullscreenControl: false,
                draggable: false,
                mapTypeId: 'satellite'
            });
            if (item.getAttribute("data-mostrar-mapa") === "True") {
                map.addMarker({
                    lat: item.getAttribute("data-latitud"),
                    lng: item.getAttribute("data-longitud"),
                });
            }
            else {
                map.addMarker({
                    lat: item.getAttribute("data-latitud"),
                    lng: item.getAttribute("data-longitud"),
                    infoWindow: {
                        content: "<div style='text-align:center'>La muestra no cuenta con coordenadas</div>"
                    },
                    visible: false,
                });
                google.maps.event.trigger(map.markers[0], 'click');
            }

        })

    }
    return {
        init: iniciar
    };
}();

jQuery(document).ready(function () {
    fueraDeLimite.init();
});
