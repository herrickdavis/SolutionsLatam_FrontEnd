var detalleReporteHistorico = function () {
    var fecha_inicio = moment(new Date());
    var fecha_fin = moment(new Date());
    var iniciar = function () {
        inicializarEvento();
    }
    var inicializarEvento = function () {
        $("#id_estacion").on("change", obtenerParametros);
        $("#id_tipo_muestra").on("change", obtenerEstaciones);
        $(".btn.btn-info.btn-lg.btn-block").on("click", obtenerReporte);
        $(".btn-group .btn.btn-success").on("click", descargarData);
    }
    var obtenerEstaciones = function () {
        var model = {
            fecha_inicio: fecha_inicio.format('YYYY-MM-DD'),
            fecha_fin: fecha_fin.format('YYYY-MM-DD'),
            id_tipo_muestra: $(this).val(),
        }
        $.post(urlObtenerEstacionesYLimites, model)
            .then(response => {
                $("#id_estacion, #id_limite, #id_parametro").empty();

                if (response.limite.length > 0) {
                    $("#divNormas").show("fast");
                    response.limite.forEach(x => {
                        var option = document.createElement("OPTION");
                        option.value = x.value;
                        option.innerText = x.text;
                        $("#id_limite").append(option);
                    });
                } else {
                    $("#id_limite").empty();
                    $("#divNormas").hide("fast");
                }

                response.estacion.forEach(item => {
                    var option = document.createElement("OPTION");
                    option.value = item.id;
                    option.innerText = item.nombre_estacion;
                    $("#id_estacion").append(option);
                })
                $('#id_estacion,#id_limite, #id_parametro').selectpicker('refresh');

            })
            .catch(error => console.log(error));
    }
    var obtenerParametros = function () {
        var model = {
            fecha_inicio: fecha_inicio.format('YYYY-MM-DD'),
            fecha_fin: fecha_fin.format('YYYY-MM-DD'),
            id_tipo_muestra: $("#id_tipo_muestra").val(),
            estaciones: $(this).val()
        }
        $.post(urlObtenerParametros, model)
            .then(response => {
                $("#id_parametro").empty();
                response.forEach(item => {
                    var option = document.createElement("OPTION");
                    option.value = item.id;
                    option.innerText = item.nombre_parametro;
                    $("#id_parametro").append(option);
                })
                $('#id_parametro').selectpicker('refresh');
            })
            .catch(error => console.log(error));
    }
    var obtenerReporte = function () {
        var model = {
            fecha_inicio: fecha_inicio.format('YYYY-MM-DD'),
            fecha_fin: fecha_fin.format('YYYY-MM-DD'),
            id_tipo_muestra: $("#id_tipo_muestra").val(),
            id_limite: $("#id_limite").val(),
            estaciones: $("#id_estacion").val(),
            parametros: $("#id_parametro").val(),
        }
        $.post(urlObtenerGrafico, model, ".card-body")
            .then(response => {
                armarGraficoParametros(response);
            })
    }
    var armarGraficoParametros = function (response) {
        $("#divGrafico").empty();
        response.graficas.forEach((item, index) => {
            var div = document.createElement("DIV");
            div.className = "mb-7 text-center";
            $("#divGrafico").append(div);

            var p = document.createElement("p");
            p.innerText = item.titulo;
            p.className = "font-weight-bold h3";
            $(div).append(p);

            var DivGrafico = document.createElement("DIV");
            DivGrafico.style.height = "450px";
            DivGrafico.id = "Div_" + index;
            $(div).append(DivGrafico);
            var result = agregarDisenoGrafico(DivGrafico.id, item, response.dates);
        })
    }
    var agregarDisenoGrafico = function (id, item, dates) {
        var data = item.series.select(x => {
            return {
                name: x.nombre, type: "line",
                data: x.datos,
                connectNulls: true,
                symbolSize: 7,
                showSymbol: x.showSymbol === undefined ? true : x.showSymbol,
                showAllSymbol: true
            };
        });
        var legend = data.select(x => { return x.name });
        var color = [];
        if (item.series != null && item.series.firstOrDefault().color !== undefined) {
            color = item.series.select(x => { return x.color });
        }
        var myChart = echarts.init(document.getElementById(id));
        var option = {
            legend: { data: legend },
            dataZoom: {
                start: 0,
                end: 100,
            },
            tooltip: {
                trigger: 'axis'
            },
            color: color,
            xAxis: {
                type: 'category',
                data: dates,
                name: item.ejex,
            },
            yAxis: {
                type: 'value',
                scale: true,
                name: item.ejey,
            },
            series: data
        };
        myChart.setOption(option);
    }
    var descargarData = function () {
        var model = {
            fecha_inicio: fecha_inicio.format('YYYY-MM-DD'),
            fecha_fin: fecha_fin.format('YYYY-MM-DD'),
            id_tipo_muestra: $("#id_tipo_muestra").val(),
            id_limite: $("#id_limite").val(),
            estaciones: $("#id_estacion").val(),
            parametros: $("#id_parametro").val(),
        }

        descargarArchivo(urlGetDataHistoricaExcel, 'post', model);
    }
    return {
        init: iniciar
    };
}();
jQuery(document).ready(function () {
    detalleReporteHistorico.init();
});
