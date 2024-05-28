var detalleMuestras = function () {
  var iniciar = function () {
      //inicializarMapa();
      //armarGraficoParametros();
      //obtenerInfo();
      $("#parametros").on("change", obtenerData);
      $("#grupos").on("change", selectParametros);
      $("#generar").on("click", function () {
          generarGrafica();
          grafica2();
          grafica3();
          grafica4();
          grafica5();
      }
      );
      obtenerAllParametros();
      obtenerAllGrupoParametros();
      obtenerAllStation();
      //agregarRangoFecha();
      //grafica2();
  }
  var inicializarMapa = function () {
    var latitud = $("#map").attr("data-latitud");
    var longitud = $("#map").attr("data-longitud");
    var map = new GMaps({
      div: '#map',
      lat: latitud,
      lng: longitud,
      // zoomControl: false,
      // mapTypeControl: false,
      // scaleControl: false,
      // streetViewControl: false,
      // rotateControl: false,
      // fullscreenControl: false,
      // draggable: false,
      mapTypeId: 'satellite'
    });
    map.addMarker({
      lat: latitud,
      lng: longitud
    });
    }

    var armarGraficoParametros = function (fecha, data) {
        var result = agregarDisenoGrafico("divGrafico", fecha, data);
    }

    var directionToCategory = function(degree) {
        const directions = ['N', 'NNE', 'NE', 'ENE', 'E', 'ESE', 'SE', 'SSE', 'S', 'SSW', 'SW', 'WSW', 'W', 'WNW', 'NW', 'NNW'];
        const index = Math.round(degree / 22.5) % 16;
        return directions[index];
    }

    var isColorTooLight = function (color) {
        // Convertir el color hex a RGB
        const r = parseInt(color.substring(1, 3), 16);
        const g = parseInt(color.substring(3, 5), 16);
        const b = parseInt(color.substring(5, 7), 16);
        // Usar una fórmula de luminancia para determinar si el color es demasiado claro
        const luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
        return luminance > 0.8; // Si la luminancia es mayor que 0.8, el color es muy claro
    }

    var getRandomColor = function() {
        const letters = '0123456789ABCDEF';
        let color = '#';
        for (let i = 0; i < 6; i++) {
            // Evitar los valores 'F' que hacen que el color sea demasiado claro
            color += letters[Math.floor(Math.random() * 16)];
        }

        // Verificar si el color es demasiado claro y regenerarlo si es necesario
        while (isColorTooLight(color)) {
            color = '#';
            for (let i = 0; i < 6; i++) {
                color += letters[Math.floor(Math.random() * 16)];
            }
        }
        return color;
    }

    var calculatePercentiles = function (data, numDivisions) {
        data.sort((a, b) => a - b);
        const percentiles = [];

        for (let i = 1; i < numDivisions; i++) {
            const percentileIndex = Math.floor((i / numDivisions) * data.length);
            percentiles.push(data[percentileIndex]);
        }

        return percentiles;
    }

    var defineRanges = function (data, percentiles) {
        const ranges = [];
        let min = percentiles[0]; // El primer límite inferior es el primer percentil

        // Agrega el primer rango
        ranges.push({ min: -Infinity, max: min, label: `<= ${min.toFixed(1)}`, color: getRandomColor() });

        for (let i = 1; i < percentiles.length; i++) {
            const max = percentiles[i];
            ranges.push({ min, max, label: `${min.toFixed(1)} - ${max.toFixed(1)}`, color: getRandomColor() });
            min = max;
        }

        // Agrega el último rango
        ranges.push({ min, max: Infinity, label: `>= ${min.toFixed(1)}`, color: getRandomColor() });

        return ranges;
    }
    var dataCache = null;
    var cargarDatos = function (payload, callback) {
        if (dataCache) {
            callback(dataCache);
        } else {
            $.post(urlGetDataTelemetria, payload)
                .then(response => {
                    dataCache = response;
                    callback(response);
                })
                .catch(error => console.log(error));
        }
    };
    var charts2 = [];
    var grafica2 = function () {
        $('#contenedorPrincipal2').empty();
        var estacionSeleccionados = $('#estaciones').find('option:selected').map(function () {
            return $(this).text();
        }).get();
        var id_parametros = $('#grupo_parametros').val()
        var payload = {
            nombre_estacion: estacionSeleccionados,
            id_parametros: id_parametros
        }
        $.post(urlGetDataWindRoseTelemetria, payload)
            .then(response => {
                console.log(response)
                var datos = response.flat();
                var datosAgrupados = datos.reduce((acc, valorActual) => {
                    (acc[valorActual.parametro_id] = acc[valorActual.parametro_id] || []).push(valorActual);
                    return acc;
                }, {});

                var numeroDeParametros = Object.keys(datosAgrupados).length;
                var numeroDeFilas = Math.ceil(numeroDeParametros / 2);
                for (let fila = 0; fila < numeroDeFilas; fila++) {
                    var $row = $('<div>').addClass('row');

                    for (let col = 0; col < 2; col++) {
                        var indice = fila * 2 + col;
                        var parametro_ids = Object.keys(datosAgrupados);
                        if (indice < parametro_ids.length) {
                            var parametro_id = parametro_ids[indice];
                            var $col = $('<div>').addClass('col-6');
                            var $card = $('<div>').addClass('card card-custom gutter-b');
                            var $cardBody = $('<div>').addClass('telemetria-grafico card-body half-screen');
                            $cardBody.attr('id', `grafico2${parametro_id}`);

                            // Ícono de FontAwesome como botón de expandir
                            var $iconExpandir = $('<i>').addClass('fas fa-expand-arrows-alt btn-expandir').attr('data-target', `grafico2${parametro_id}`);
                            // Añade estilos para hacer el ícono interactivo, si es necesario
                            $iconExpandir.css({ 'cursor': 'pointer', 'margin-right': '10px' });

                            // Crea el header del card con el título y el ícono de expandir
                            var $cardHeader = $('<div>').addClass('card-header d-flex justify-content-between align-items-center');
                            var $title = $('<h1>').addClass('card-title').text(datosAgrupados[parametro_id][0].nombre_parametro);
                            $cardHeader.append($title).append($iconExpandir);

                            // Añade el header y el body al card
                            $card.append($cardHeader).append($cardBody);
                            $col.append($card);
                            $row.append($col);
                        }
                    }
                    $('#contenedorPrincipal2').append($row);
                }
                $('#graficoModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                    var modalChartContainer = document.getElementById('modalGraficoContainer');
                    modalChartContainer.style.height = '400px'; // Asegura que el contenedor tenga un tamaño

                    // Inicializa la gráfica en el modal solo si modalChartOption está definida
                    if (modalChartOption) {
                        var modalChartInstance = echarts.init(modalChartContainer);
                        modalChartInstance.setOption(modalChartOption, true); // Aplica las opciones del gráfico seleccionado
                    }
                });

                $(document).on('click', '.btn-expandir', function () {
                    var chartId = $(this).data('target');
                    var originalChartInstance = echarts.getInstanceByDom(document.getElementById(chartId));
                    modalChartOption = originalChartInstance.getOption(); // Actualiza las opciones de la gráfica seleccionada para expandir
                    $('#graficoModal').modal('show'); // Muestra el modal
                });

                var todasLasFechas = [];

                datos.forEach(item => {
                    todasLasFechas.push(item.fecha_muestreo);
                });
                var fechasUnicas = Array.from(new Set(todasLasFechas));
                //agregarRangoFecha(fechasUnicas)
                parametro_ids.forEach(parametro_id => {
                    var halfHeight = (window.innerHeight - 60) / 2;
                    document.querySelectorAll('.half-screen').forEach(function (element) {
                        element.style.height = `${halfHeight}px`;
                    });

                    var chartDom = document.getElementById('grafico2' + parametro_id);
                    var chart2 = echarts.init(chartDom);
                    charts2.push(chart2);
                    // Convertir grados a categorías
                    function directionToCategory(degree) {
                        const directions = ['N', 'NNE', 'NE', 'ENE', 'E', 'ESE', 'SE', 'SSE', 'S', 'SSO', 'SO', 'OSO', 'O', 'ONO', 'NO', 'NNO'];
                        const index = Math.round(degree / 22.5) % 16;
                        return directions[index];
                    }

                    // Función para generar colores al azar
                    function getRandomColor() {
                        return `#${Math.floor(Math.random() * 16777215).toString(16)}`;
                    }

                    // Datos específicos para la rosa de los vientos
                    var processedData = datosAgrupados[parametro_id].map(item => ({
                        direction: directionToCategory(item.windDir_D1_WVT),
                        pm25: item.resultado // Suponiendo que este es el nombre del campo para PM2.5
                    }));

                    // Número de divisiones y cálculo de rangos de PM2.5
                    const numDivisions = 6; // Puedes ajustar este valor
                    /*const pm25Values = processedData.map(item => item.pm25);
                    const minPM25 = Math.min(...pm25Values);
                    const maxPM25 = Math.max(...pm25Values);
                    const rangeSize = (maxPM25 - minPM25) / numDivisions;

                    // Crear rangos de PM2.5 con colores al azar
                    const pm25Ranges = Array.from({ length: numDivisions }, (v, i) => ({
                        min: minPM25 + i * rangeSize,
                        max: minPM25 + (i + 1) * rangeSize,
                        color: getRandomColor()
                    }));*/

                    // Calcular frecuencias en porcentajes
                    const totalDataPoints = processedData.length;
                    const directions = ['N', 'NNE', 'NE', 'ENE', 'E', 'ESE', 'SE', 'SSE', 'S', 'SSW', 'SW', 'WSW', 'W', 'WNW', 'NW', 'NNW'];

                    const pm25Values = processedData.map(item => parseFloat(item.pm25)); // Asegúrate de que los valores sean números
                    const quartiles = calculatePercentiles(pm25Values, 4);
                    const pm25Ranges = defineRanges(pm25Values, quartiles);

                    const series = pm25Ranges.map(range => ({
                        name: range.label,
                        type: 'bar',
                        data: directions.map(direction => {
                            const filtered = processedData.filter(item => item.direction === direction && item.pm25 >= range.min && item.pm25 < range.max);
                            const percentage = (filtered.length / totalDataPoints) * 100;
                            return percentage;
                        }),
                        coordinateSystem: 'polar',
                        stack: 'a',
                        itemStyle: {
                            color: range.color
                        }
                    }));

                    // Configurar el gráfico
                    var option = {
                        tooltip: {
                            trigger: 'item',
                            formatter: function (params) {
                                return `${params.seriesName}<br/>${params.marker} ${params.name}: ${params.value.toFixed(2)}%`;
                            }
                        },
                        angleAxis: {
                            type: 'category',
                            data: directions,
                            startAngle: 101.25 // Ajusta el ángulo inicial para que 'N' esté en la parte superior
                        },
                        radiusAxis: {
                            axisLabel: {
                                formatter: '{value}%',
                                show: true,
                                z: 10
                            },
                            z: 10 // Aumentar z-index para que el eje esté por encima de las barras
                        },
                        polar: {
                            center: ['40%', '50%'], // Mueve el gráfico hacia la izquierda (['x%', 'y%'])
                            radius: '70%'
                        },
                        series: series,
                        legend: {
                            show: true,
                            data: series.map(serie => serie.name),
                            right: '10%',  // Ajusta esto para mover la leyenda a la derecha
                            top: 'middle', // Centra verticalmente
                            orient: 'vertical' 
                        }
                    };

                    chart2.setOption(option);
                });
                //syncDataZoomFromMaster('range_fecha');
            })
            .catch(error => console.log(error));
    }
    var charts1 = []
    var generarGrafica = function () {
        $('#contenedorPrincipal').empty();
        var estacionSeleccionados = $('#estaciones').find('option:selected').map(function () {
            return $(this).text();
        }).get();
        var id_parametros = $('#grupo_parametros').val()
        var payload = {
            nombre_estacion : estacionSeleccionados,
            id_parametros : id_parametros
        }
        cargarDatos(payload, function (datos) {
            var datosAgrupados = datos.reduce((acc, valorActual) => {
                (acc[valorActual.parametro_id] = acc[valorActual.parametro_id] || []).push(valorActual);
                return acc;
            }, {});

            var numeroDeParametros = Object.keys(datosAgrupados).length;
            var numeroDeFilas = Math.ceil(numeroDeParametros / 2);
            for (let fila = 0; fila < numeroDeFilas; fila++) {
                var $row = $('<div>').addClass('row');

                for (let col = 0; col < 2; col++) {
                    var indice = fila * 2 + col;
                    var parametro_ids = Object.keys(datosAgrupados);
                    if (indice < parametro_ids.length) {
                        var parametro_id = parametro_ids[indice];
                        var $col = $('<div>').addClass('col-6');
                        var $card = $('<div>').addClass('card card-custom gutter-b');
                        var $cardBody = $('<div>').addClass('telemetria-grafico card-body half-screen');
                        $cardBody.attr('id', `grafico${parametro_id}`);

                        // Ícono de FontAwesome como botón de expandir
                        var $iconExpandir = $('<i>').addClass('fas fa-expand-arrows-alt btn-expandir').attr('data-target', `grafico${parametro_id}`);
                        // Añade estilos para hacer el ícono interactivo, si es necesario
                        $iconExpandir.css({ 'cursor': 'pointer', 'margin-right': '10px' });

                        // Crea el header del card con el título y el ícono de expandir
                        var $cardHeader = $('<div>').addClass('card-header d-flex justify-content-between align-items-center');
                        var $title = $('<h1>').addClass('card-title').text(datosAgrupados[parametro_id][0].nombre_parametro);
                        $cardHeader.append($title).append($iconExpandir);

                        // Añade el header y el body al card
                        $card.append($cardHeader).append($cardBody);
                        $col.append($card);
                        $row.append($col);
                    }
                }
                $('#contenedorPrincipal').append($row);
            }
            $('#graficoModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                var modalChartContainer = document.getElementById('modalGraficoContainer');
                //modalChartContainer.innerHTML = ''; // Limpia el contenedor
                modalChartContainer.style.height = '400px'; // Asegura que el contenedor tenga un tamaño

                // Inicializa la gráfica en el modal solo si modalChartOption está definida
                if (modalChartOption) {
                    var modalChartInstance = echarts.init(modalChartContainer);
                    modalChartInstance.setOption(modalChartOption, true); // Aplica las opciones del gráfico seleccionado
                }
            });

            $(document).on('click', '.btn-expandir', function () {
                var chartId = $(this).data('target');
                var originalChartInstance = echarts.getInstanceByDom(document.getElementById(chartId));
                modalChartOption = originalChartInstance.getOption(); // Actualiza las opciones de la gráfica seleccionada para expandir
                $('#graficoModal').modal('show'); // Muestra el modal
            });

            var todasLasFechas = [];

            datos.forEach(item => {
                todasLasFechas.push(item.fecha_muestreo);
            });
            var fechasUnicas = Array.from(new Set(todasLasFechas));
            agregarRangoFecha(fechasUnicas)
            parametro_ids.forEach(parametro_id => {
                var halfHeight = (window.innerHeight - 60) / 2;
                document.querySelectorAll('.half-screen').forEach(function (element) {
                    element.style.height = `${halfHeight}px`;
                });

                var chartDom = document.getElementById('grafico' + parametro_id);
                var chart1 = echarts.init(chartDom);
                charts1.push(chart1);
                var series = [];
                var legendData = [];

                var datosPorEstacion = datosAgrupados[parametro_id].reduce((acc, item) => {
                    if (!acc[item.nombre_estacion]) {
                        acc[item.nombre_estacion] = [];
                    }
                    acc[item.nombre_estacion].push([item.fecha_muestreo, item.resultado]);
                    return acc;
                }, {});

                Object.keys(datosPorEstacion).forEach(nombre_estacion => {
                    series.push({
                        name: nombre_estacion,
                        type: 'line',
                        data: datosPorEstacion[nombre_estacion],
                        symbol: 'none',
                        sampling: 'average',
                        smooth: true,
                    });
                    legendData.push(nombre_estacion);
                });

                var option = {
                    tooltip: {
                        trigger: 'axis',
                        position: function (pt) {
                            return [pt[0], '10%'];
                        }
                    },
                    toolbox: {
                    },
                    legend: {
                        data: legendData
                    },
                    xAxis: {
                        type: 'time',
                        boundaryGap: false,
                    },
                    yAxis: {
                        type: 'value'
                    },
                    dataZoom: [
                        {
                            type: 'inside',
                            start: 80,
                            end: 100
                        },
                        {
                            start: 80,
                            end: 100
                        }
                    ],
                    series: series
                };
                chart1.setOption(option);
            });
            syncDataZoomFromMaster('range_fecha');
        });
    }
    var charts3 = []
    var grafica3 = function () {
        $('#contenedorPrincipal3').empty();
        var estacionSeleccionados = $('#estaciones').find('option:selected').map(function () {
            return $(this).text();
        }).get();
        var id_parametros = $('#grupo_parametros').val();
        var payload = {
            nombre_estacion: estacionSeleccionados,
            id_parametros: id_parametros
        };
        cargarDatos(payload, function (datos) {
            var datosAgrupados = datos.reduce((acc, valorActual) => {
                (acc[valorActual.parametro_id] = acc[valorActual.parametro_id] || []).push(valorActual);
                return acc;
            }, {});

            var numeroDeParametros = Object.keys(datosAgrupados).length;
            var numeroDeFilas = Math.ceil(numeroDeParametros / 2);
            for (let fila = 0; fila < numeroDeFilas; fila++) {
                var $row = $('<div>').addClass('row');

                for (let col = 0; col < 2; col++) {
                    var indice = fila * 2 + col;
                    var parametro_ids = Object.keys(datosAgrupados);
                    if (indice < parametro_ids.length) {
                        var parametro_id = parametro_ids[indice];
                        var $col = $('<div>').addClass('col-6');
                        var $card = $('<div>').addClass('card card-custom gutter-b');
                        var $cardBody = $('<div>').addClass('telemetria-grafico card-body half-screen');
                        $cardBody.attr('id', `grafico3${parametro_id}`);

                        // Ícono de FontAwesome como botón de expandir
                        var $iconExpandir = $('<i>').addClass('fas fa-expand-arrows-alt btn-expandir').attr('data-target', `grafico3${parametro_id}`);
                        $iconExpandir.css({ 'cursor': 'pointer', 'margin-right': '10px' });

                        // Crea el header del card con el título y el ícono de expandir
                        var $cardHeader = $('<div>').addClass('card-header d-flex justify-content-between align-items-center');
                        var $title = $('<h1>').addClass('card-title').text(datosAgrupados[parametro_id][0].nombre_parametro);
                        $cardHeader.append($title).append($iconExpandir);

                        // Añade el header y el body al card
                        $card.append($cardHeader).append($cardBody);
                        $col.append($card);
                        $row.append($col);
                    }
                }
                $('#contenedorPrincipal3').append($row);
            }
            $('#graficoModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                var modalChartContainer = document.getElementById('modalGraficoContainer');
                modalChartContainer.style.height = '400px';

                if (modalChartOption) {
                    var modalChartInstance = echarts.init(modalChartContainer);
                    modalChartInstance.setOption(modalChartOption, true);
                }
            });

            $(document).on('click', '.btn-expandir', function () {
                var chartId = $(this).data('target');
                var originalChartInstance = echarts.getInstanceByDom(document.getElementById(chartId));
                modalChartOption = originalChartInstance.getOption();
                $('#graficoModal').modal('show');
            });

            var todasLasFechas = [];
            datos.forEach(item => {
                todasLasFechas.push(item.fecha_muestreo);
            });
            var fechasUnicas = Array.from(new Set(todasLasFechas));
            //agregarRangoFecha(fechasUnicas);

            parametro_ids.forEach(parametro_id => {
                var halfHeight = (window.innerHeight - 60) / 2;
                document.querySelectorAll('.half-screen').forEach(function (element) {
                    element.style.height = `${halfHeight}px`;
                });

                var chartDom = document.getElementById('grafico3' + parametro_id);
                var chart3 = echarts.init(chartDom);
                charts3.push(chart3);

                var series = [];
                var legendData = [];
                var categorias = []; // Array para almacenar las categorías del eje X

                var datosPorEstacion = datosAgrupados[parametro_id].reduce((acc, item) => {
                    if (!acc[item.nombre_estacion]) {
                        acc[item.nombre_estacion] = [];
                    }
                    acc[item.nombre_estacion].push(parseFloat(item.resultado));
                    return acc;
                }, {});

                Object.keys(datosPorEstacion).forEach(nombre_estacion => {
                    categorias.push(nombre_estacion); // Añadir el nombre de la estación a las categorías
                });

                categorias.forEach(nombre_estacion => {
                    const valores = datosPorEstacion[nombre_estacion].sort((a, b) => a - b);
                    const min = valores[0];
                    const q1 = valores[Math.floor((valores.length / 4))];
                    const median = valores[Math.floor((valores.length / 2))];
                    const q3 = valores[Math.floor((3 * valores.length) / 4)];
                    const max = valores[valores.length - 1];
                    series.push({
                        name: nombre_estacion,
                        type: 'boxplot',
                        data: [[min, q1, median, q3, max]]
                    });
                    legendData.push(nombre_estacion);
                });

                var option = {
                    title: {
                        //text: datosAgrupados[parametro_id][0].nombre_parametro
                    },
                    tooltip: {
                        trigger: 'item',
                        axisPointer: {
                            type: 'shadow'
                        }
                    },
                    legend: {
                        data: legendData
                    },
                    xAxis: {
                        type: 'category',
                        data: [] // Se utiliza `categorias` para el eje X
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: series
                };
                chart3.setOption(option);
            });
            //syncDataZoomFromMaster('range_fecha');
        });            
    }
    var charts4 = []
    var grafica4 = function () {
        $('#contenedorPrincipal4').empty();
        var estacionSeleccionados = $('#estaciones').find('option:selected').map(function () {
            return $(this).text();
        }).get();
        var id_parametros = $('#grupo_parametros').val();
        var payload = {
            nombre_estacion: estacionSeleccionados,
            id_parametros: id_parametros
        };
        cargarDatos(payload, function (datos) {
            var datosAgrupados = datos.reduce((acc, valorActual) => {
                (acc[valorActual.parametro_id] = acc[valorActual.parametro_id] || []).push(valorActual);
                return acc;
            }, {});

            var numeroDeParametros = Object.keys(datosAgrupados).length;
            var numeroDeFilas = Math.ceil(numeroDeParametros / 2);
            for (let fila = 0; fila < numeroDeFilas; fila++) {
                var $row = $('<div>').addClass('row');
                for (let col = 0; col < 2; col++) {
                    var indice = fila * 2 + col;
                    var parametro_ids = Object.keys(datosAgrupados);
                    if (indice < parametro_ids.length) {
                        var parametro_id = parametro_ids[indice];
                        var $col = $('<div>').addClass('col-6');
                        var $card = $('<div>').addClass('card card-custom gutter-b');
                        var $cardBody = $('<div>').addClass('telemetria-grafico card-body half-screen');
                        $cardBody.attr('id', `grafico4${parametro_id}`);

                        var $iconExpandir = $('<i>').addClass('fas fa-expand-arrows-alt btn-expandir').attr('data-target', `grafico4${parametro_id}`);
                        $iconExpandir.css({ 'cursor': 'pointer', 'margin-right': '10px' });

                        var $cardHeader = $('<div>').addClass('card-header d-flex justify-content-between align-items-center');
                        var $title = $('<h1>').addClass('card-title').text(datosAgrupados[parametro_id][0].nombre_parametro);
                        $cardHeader.append($title).append($iconExpandir);

                        $card.append($cardHeader).append($cardBody);
                        $col.append($card);
                        $row.append($col);
                    }
                }
                $('#contenedorPrincipal4').append($row);
            }
            $('#graficoModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                var modalChartContainer = document.getElementById('modalGraficoContainer');
                modalChartContainer.style.height = '400px';

                if (modalChartOption) {
                    var modalChartInstance = echarts.init(modalChartContainer);
                    modalChartInstance.setOption(modalChartOption, true);
                }
            });

            $(document).on('click', '.btn-expandir', function () {
                var chartId = $(this).data('target');
                var originalChartInstance = echarts.getInstanceByDom(document.getElementById(chartId));
                modalChartOption = originalChartInstance.getOption();
                $('#graficoModal').modal('show');
            });

            var todasLasFechas = [];
            datos.forEach(item => {
                todasLasFechas.push(item.fecha_muestreo);
            });
            var fechasUnicas = Array.from(new Set(todasLasFechas));
            //agregarRangoFecha(fechasUnicas);

            var halfHeight = (window.innerHeight - 60) / 2;
            document.querySelectorAll('.half-screen').forEach(function (element) {
                element.style.height = `${halfHeight}px`;
            });

            Object.keys(datosAgrupados).forEach(parametro_id => {
                var chartDom = document.getElementById('grafico4' + parametro_id);
                var chart4 = echarts.init(chartDom);
                charts4.push(chart4);

                var series = [];
                var legendData = [];
                var categorias = [];

                var datosPorEstacion = datosAgrupados[parametro_id].reduce((acc, item) => {
                    if (!acc[item.nombre_estacion]) {
                        acc[item.nombre_estacion] = [];
                    }
                    acc[item.nombre_estacion].push({ estacion: item.nombre_estacion, valor: parseFloat(item.resultado) });
                    return acc;
                }, {});

                var allData = [];

                Object.keys(datosPorEstacion).forEach(nombre_estacion => {
                    categorias.push(nombre_estacion);
                    const valores = datosPorEstacion[nombre_estacion];

                    valores.forEach(v => {
                        allData.push([nombre_estacion, v.valor]);
                    });
                });

                var minValor = Math.min(...datos.map(d => d.resultado));
                var maxValor = Math.max(...datos.map(d => d.resultado));
                var intervalo = 10;
                var rangos = [];

                for (var i = Math.floor(minValor); i <= Math.ceil(maxValor); i += intervalo) {
                    rangos.push(i);
                }

                var datosPorRango = datos.reduce((acc, valorActual) => {
                    var rango = rangos.find(r => valorActual.resultado <= r);
                    if (!acc[valorActual.nombre_estacion]) {
                        acc[valorActual.nombre_estacion] = {};
                    }
                    if (!acc[valorActual.nombre_estacion][rango]) {
                        acc[valorActual.nombre_estacion][rango] = 0;
                    }
                    acc[valorActual.nombre_estacion][rango]++;
                    return acc;
                }, {});

                var maxFrecuencia = Math.max(...Object.values(datosPorRango).flatMap(d => Object.values(d)));
                var seriesData = [];
                for (var estacion in datosPorRango) {
                    for (var rango in datosPorRango[estacion]) {
                        seriesData.push({
                            name: estacion,
                            value: [estacion, parseInt(rango), datosPorRango[estacion][rango]],
                            symbolSize: (Math.sqrt(datosPorRango[estacion][rango]) / Math.sqrt(maxFrecuencia)) * 50
                        });
                    }
                }

                series.push({
                    name: 'Scatter',
                    type: 'scatter',
                    data: seriesData
                });

                var option = {
                    title: {},
                    tooltip: {
                        trigger: 'item',
                        formatter: function (params) {
                            return `Estación: ${params.value[0]}<br/>Rango: ${params.value[1]}<br/>Frecuencia: ${params.value[2]}`;
                        }
                    },
                    xAxis: {
                        type: 'category',
                        data: categorias,
                        name: 'Estación'
                    },
                    yAxis: {
                        type: 'value',
                        name: 'Rango de PM2.5',
                        data: rangos.map(r => `${r - intervalo / 2}-${r + intervalo / 2}`)
                    },
                    series: series
                };
                chart4.setOption(option);
            });

            //syncDataZoomFromMaster('range_fecha');
        });
    };
    var charts5 = []
    var grafica5 = function () {
        $('#contenedorPrincipal5').empty();
        var estacionSeleccionados = $('#estaciones').find('option:selected').map(function () {
            return $(this).text();
        }).get();
        var id_parametros = $('#grupo_parametros').val();
        var mesSeleccionado = $('#selectMes').val();
        var payload = {
            nombre_estacion: estacionSeleccionados,
            id_parametros: id_parametros
        };
        cargarDatos(payload, function (datos) {
            var datosAgrupados = datos.reduce((acc, valorActual) => {
                (acc[valorActual.parametro_id] = acc[valorActual.parametro_id] || []).push(valorActual);
                return acc;
            }, {});

            var numeroDeParametros = Object.keys(datosAgrupados).length;
            var numeroDeFilas = Math.ceil(numeroDeParametros / 2);
            for (let fila = 0; fila < numeroDeFilas; fila++) {
                var $row = $('<div>').addClass('row');

                for (let col = 0; col < 2; col++) {
                    var indice = fila * 2 + col;
                    var parametro_ids = Object.keys(datosAgrupados);
                    if (indice < parametro_ids.length) {
                        var parametro_id = parametro_ids[indice];
                        var $col = $('<div>').addClass('col-6');
                        var $card = $('<div>').addClass('card card-custom gutter-b');
                        var $cardBody = $('<div>').addClass('telemetria-grafico card-body half-screen');
                        $cardBody.attr('id', `grafico5${parametro_id}`);

                        var $iconExpandir = $('<i>').addClass('fas fa-expand-arrows-alt btn-expandir').attr('data-target', `grafico5${parametro_id}`);
                        $iconExpandir.css({ 'cursor': 'pointer', 'margin-right': '10px' });

                        var $cardHeader = $('<div>').addClass('card-header d-flex justify-content-between align-items-center');
                        var $title = $('<h1>').addClass('card-title').text(datosAgrupados[parametro_id][0]?.nombre_parametro || 'Sin Nombre');
                        $cardHeader.append($title).append($iconExpandir);

                        $card.append($cardHeader).append($cardBody);
                        $col.append($card);
                        $row.append($col);
                    }
                }
                $('#contenedorPrincipal5').append($row);
            }
            $('#graficoModal').off('shown.bs.modal').on('shown.bs.modal', function () {
                var modalChartContainer = document.getElementById('modalGraficoContainer');
                modalChartContainer.style.height = '400px';

                if (modalChartOption) {
                    var modalChartInstance = echarts.init(modalChartContainer);
                    modalChartInstance.setOption(modalChartOption, true);
                }
            });

            $(document).on('click', '.btn-expandir', function () {
                var chartId = $(this).data('target');
                var originalChartInstance = echarts.getInstanceByDom(document.getElementById(chartId));
                modalChartOption = originalChartInstance.getOption();
                $('#graficoModal').modal('show');
            });

            var todasLasFechas = [];
            datos.forEach(item => {
                todasLasFechas.push(item.fecha_muestreo);
            });
            var fechasUnicas = Array.from(new Set(todasLasFechas));
            //agregarRangoFecha(fechasUnicas);

            var fechaActual = new Date();
            fechaActual.setMonth(mesSeleccionado);
            var mesString = fechaActual.toISOString().split('T')[0].slice(0, 7);

            var halfHeight = (window.innerHeight - 60) / 2;
            document.querySelectorAll('.half-screen').forEach(function (element) {
                element.style.height = `${halfHeight}px`;
            });

            Object.keys(datosAgrupados).forEach(parametro_id => {
                var chartDom = document.getElementById('grafico5' + parametro_id);
                var chart5 = echarts.init(chartDom);
                charts5.push(chart5);

                var datosPorEstacion = datosAgrupados[parametro_id].reduce((acc, item) => {
                    if (!acc[item.nombre_estacion]) {
                        acc[item.nombre_estacion] = [];
                    }
                    acc[item.nombre_estacion].push({ fecha: item.fecha_muestreo.split(' ')[0], valor: parseFloat(item.resultado), estacion: item.nombre_estacion });
                    return acc;
                }, {});

                var heatmapData = [];
                var scatterData = [];

                Object.keys(datosPorEstacion).forEach(estacion => {
                    var datosPorFecha = datosPorEstacion[estacion].reduce((acc, item) => {
                        if (!acc[item.fecha]) {
                            acc[item.fecha] = item.valor;
                        } else {
                            acc[item.fecha] = Math.max(acc[item.fecha], item.valor);
                        }
                        return acc;
                    }, {});

                    Object.keys(datosPorFecha).forEach(fecha => {
                        if (fecha.startsWith(mesString)) {
                            heatmapData.push([fecha, datosPorFecha[fecha], estacion]);
                            scatterData.push([fecha, datosPorFecha[fecha], estacion]);
                        }
                    });
                });

                var minValor = Math.min(...heatmapData.map(d => d[1]));
                var maxValor = Math.max(...heatmapData.map(d => d[1]));

                var option = {
                    tooltip: {
                        position: 'top',
                        formatter: function (params) {
                            return `Fecha: ${params.value[0]}<br/>Estación: ${params.value[2]}<br/>Valor Máximo: ${params.value[1]}`;
                        }
                    },
                    visualMap: {
                        min: minValor,
                        max: maxValor,
                        calculable: true,
                        orient: 'horizontal',
                        left: 'center',
                        bottom: '15%',
                        show: false
                    },
                    calendar: {
                        range: mesString,
                        left: 'center',
                        top: 'middle',
                        orient: 'vertical',
                        cellSize: [60, 60],
                        splitLine: {
                            show: false,
                            lineStyle: {
                                color: '#ddd',
                                width: 1,
                                type: 'solid'
                            }
                        },
                        itemStyle: {
                            borderWidth: 1,
                            borderColor: '#ccc'
                        },
                        dayLabel: {
                            firstDay: 1,
                            nameMap: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb']
                        },
                        monthLabel: {
                            show: true,
                            nameMap: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre']
                        },
                        yearLabel: {
                            show: false
                        }
                    },
                    series: [{
                        type: 'heatmap',
                        coordinateSystem: 'calendar',
                        data: heatmapData.map(item => [item[0], item[1]])
                    },
                    {
                        type: 'scatter',
                        coordinateSystem: 'calendar',
                        symbolSize: 0,
                        label: {
                            show: true,
                            formatter: function (params) {
                                var d = echarts.number.parseDate(params.value[0]);
                                return d.getDate() + '\n\n';
                            },
                            color: '#000',
                            fontWeight: 'bold'
                        },
                        data: scatterData,
                        silent: true
                    },
                    {
                        type: 'scatter',
                        coordinateSystem: 'calendar',
                        symbolSize: 0,
                        label: {
                            show: true,
                            formatter: function (params) {
                                var d = echarts.number.parseDate(params.value[0]);
                                return '\n\n' + params.value[1].toFixed(2);
                            },
                            color: '#000'
                        },
                        data: scatterData,
                        silent: true
                    }
                    ]
                };

                chart5.setOption(option);
            });

            //syncDataZoomFromMaster('range_fecha');
        });
    };

    // Llama a la función cuando cambie el selector
    $('#selectMes').on('change', grafica5);

    var modalChartOption = null;

    $('#graficoModal').on('hidden.bs.modal', function () {
        modalChartOption = null; // Limpia la variable después de que el modal se cierra
    });

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("data-target"); // Activo tab
        if (target === '#grafica1') {
            charts1.forEach(chart => {
                chart.resize();
            });
        } else if (target === '#grafica2') {
            charts2.forEach(chart => {
                chart.resize();
            });
        } else if (target === '#grafica3') {
            charts3.forEach(chart => {
                chart.resize();
            });
        } else if (target === '#grafica4') {
            charts4.forEach(chart => {
                chart.resize();
            });
        } else if (target === '#grafica5') {
            charts5.forEach(chart => {
                chart.resize();
            });
        }
    });

    var agregarDisenoGrafico = function (id, fecha, data) {

        var chartDom = document.getElementById(id);

        var myChart = echarts.init(chartDom);

        var option;

        option = {
            tooltip: {
                trigger: 'axis',
                position: function (pt) {
                    return [pt[0], '10%'];
                }
            },
            toolbox: {
                feature: {
                    dataZoom: {
                        yAxisIndex: 'none'
                    },
                    restore: {},
                    saveAsImage: {}
                }
            },
            xAxis: {
                type: 'time',
                boundaryGap: false,
            },
            yAxis: {
                type: 'value'
            },
            dataZoom: [
                {
                    type: 'inside',
                    start: 80,
                    end: 100
                },
                {
                    start: 0,
                    end: 20
                }
            ],
            series: [
                {
                    data: data,
                    type: 'line',
                    symbol: 'none',
                    sampling: 'average',
                    smooth: true,
                }
            ]
        };
        option && myChart.setOption(option);
    }

    var parseCustomDateFormat = function (dateString) {
        var parts = dateString.split(' ');
        var dateParts = parts[0].split('-');
        var timeParts = parts[1].split(':');

        var year = parseInt(dateParts[0], 10);
        var month = parseInt(dateParts[1], 10) - 1; // Los meses en JavaScript son base-0
        var day = parseInt(dateParts[2], 10);
        var hours = parseInt(timeParts[0], 10);
        var minutes = parseInt(timeParts[1], 10);
        var seconds = parseInt(timeParts[2], 10);

        return new Date(year, month, day, hours, minutes, seconds);
    }

    var uploadLimite = function(id = null) {
        var nombre_limite = $("#nombre_limite").val()

        var rows = document.querySelectorAll('#rowsContainer .row');
        var datosParaEnviar = [];

        rows.forEach(function (row, index) {
            var grupo = $(row).find('.grupo_parametros2').val();
            var limiteInferior = $(row).find('.limite_inferior').val();
            var limiteSuperior = $(row).find('.limite_superior').val();
            var rowIndex = $(row).find('.row-index').val();

            if (grupo && (limiteInferior || limiteSuperior)) {
                datosParaEnviar.push({
                    id_parametro: grupo,
                    limite_inferior: limiteInferior,
                    limite_superior: limiteSuperior
                });
            }
        });
        if (id != null) {
            model = {
                id_limite : id,
                nombre_limite: nombre_limite,
                parametros: datosParaEnviar
            }
        } else {
            model = {
                nombre_limite: nombre_limite,
                parametros: datosParaEnviar
            }
        }
        
        $.post(urlSetLimiteTelemetria, model)
            .then(response => {
                toastr.success(decodeURIComponent(JSON.parse('"' + response.replace(/\"/g, '\\"') + '"')));
                obtenerLimite()
                //Quito el modal
                $('#crearLimiteModal').modal('hide')
            })
            .catch(error => console.log(error));

        console.log(datosParaEnviar)

    }
    var parseDate = function(str) {
        var parts = str.split(' ')[0].split('-'); // Asume formato "Y/M/D H:M:S" y solo toma la parte de la fecha
        return new Date(parts[0], parts[1] - 1, parts[2]); // Ajusta el mes (-1 porque en JS es base 0)
    }

    var agregarRangoFecha = function (date) {
        let mostRecentDate = parseDate(date[date.length - 1]);
        let sevenDaysAgo = new Date(mostRecentDate.getFullYear(), mostRecentDate.getMonth(), mostRecentDate.getDate() - 7);
        let startIndex = date.findIndex(d => parseDate(d) >= sevenDaysAgo);
        let endIndex = date.length - 1; // El último índice, asumiendo que 'date' está ordenado cronológicamente
        var chartDom = document.getElementById('range_fecha');
        var myChart = echarts.init(chartDom);
        var option;
        var date = date
        option = {
            xAxis: {
                show: false,
                data: date
            },
            yAxis: {
                show: false
            },
            dataZoom: [
                {
                    type: 'slider',
                    startValue: startIndex,
                    endValue: endIndex,
                    labelFormatter: function (index) {
                        // 'index' es el índice en el arreglo de fechas, usalo para obtener la fecha correspondiente
                        var dateString = date[index]; // Accede a la fecha usando el índice
                        if (!dateString) {
                            return "";
                        }
                        // Asume que 'parseCustomDateFormat' puede manejar tus fechas en formato "Y/M/D H:M:S"
                        var dateObject = parseCustomDateFormat(dateString);
                        var formattedDate = dateObject.getDate() + '/' + (dateObject.getMonth() + 1) + '/' + dateObject.getFullYear();
                        return formattedDate;
                    }
                }
            ],
        };
        option && myChart.setOption(option);
    }

    /*var getAllDynamicChartInstances = function() {
        var allElements = document.querySelectorAll('[id]'); // Selecciona todos los elementos con `id`
        var chartInstances = [];
        var pattern = /^(grafica1|grafico)\d+$/; // Expresión regular para coincidir con tus `id`

        allElements.forEach(function (element) {
            if (pattern.test(element.id)) {
                var instance = echarts.getInstanceByDom(element);
                if (instance) {
                    chartInstances.push(instance);
                }
            }
        });

        return chartInstances;
    }*/

    var getAllDynamicChartInstances = function () {
        var container = document.getElementById('grafica1'); // Selecciona el contenedor específico
        var allElements = container.querySelectorAll('[id]'); // Selecciona todos los elementos con `id` dentro del contenedor
        var chartInstances = [];
        var pattern = /^grafico\d+$/; // Expresión regular para coincidir con tus `id` específicos

        allElements.forEach(function (element) {
            if (pattern.test(element.id)) {
                var instance = echarts.getInstanceByDom(element);
                if (instance) {
                    chartInstances.push(instance);
                }
            }
        });

        return chartInstances;
    };

    var syncDataZoomFromMaster = function(masterChartId) {
        var masterChart = getChartInstanceById(masterChartId);
        if (!masterChart) {
            console.error("Gráfica maestra no encontrada.");
            return; // Sale si no se encuentra la gráfica maestra
        }
        var allCharts = getAllDynamicChartInstances(); // Obtiene todas las instancias como antes
        masterChart.on('dataZoom', function () {
            var option = masterChart.getOption();
            console.log(option)
            var zoomOption = {
                dataZoom: [{
                    start: option.dataZoom[0].start,
                    end: option.dataZoom[0].end
                }]
            };
            allCharts.forEach(function (chart) {
                if (chart !== masterChart) { // Aplica la configuración a todas las gráficas excepto la maestra
                    chart.setOption(zoomOption);
                }
            });
        });
    }

    var getChartInstanceById = function(id) {
        var element = document.getElementById(id);
        if (element) {
            return echarts.getInstanceByDom(element);
        }
        return null; // Retorna nulo si el elemento con ese ID no existe
    }

    document.getElementById('rangeSelector').addEventListener('change', function () {
        var masterChart = getChartInstanceById('range_fecha');
        var option = masterChart.getOption();
        var xAxisData = option.xAxis[0].data.map(d => new Date(d));

        // Mostrar los datos del eje X en la consola
        //console.log(xAxisData);
        var days = parseInt(this.value);
        var mostRecentDate = xAxisData[xAxisData.length - 1];
        var targetDate = new Date(mostRecentDate);
        targetDate.setDate(targetDate.getDate() - days);

        // Encontrar el índice del targetDate más cercano en xAxisData
        var startIndex = xAxisData.findIndex(date => {
            return date.getFullYear() === targetDate.getFullYear() &&
                date.getMonth() === targetDate.getMonth() &&
                date.getDate() === targetDate.getDate();
        });

        // Si no se encuentra la fecha exacta, buscar la más cercana
        if (startIndex === -1) {
            startIndex = xAxisData.reduce((closestIndex, currentDate, currentIndex) => {
                var currentDiff = Math.abs(currentDate - targetDate);
                var closestDiff = Math.abs(xAxisData[closestIndex] - targetDate);
                return currentDiff < closestDiff ? currentIndex : closestIndex;
            }, 0);
        }
        console.log(startIndex)
        console.log(xAxisData.length - 1)
        
        var zoomOption = {
            dataZoom: [{
                type: 'slider',
                startValue: startIndex, // Valor Unix del startIndex
                endValue: xAxisData.length - 1 // Valor Unix del último elemento
            }]
        };
        masterChart.setOption(zoomOption);
        console.log(masterChart.getOption())
    });

    var infoTelemetria = ''
    var obtenerInfo = function () {
        $.post(urlGetInfoTelemetria, null)
            .then(response => {
                infoTelemetria = response
                $("#estaciones").empty();
                response.forEach(item => {
                    var option = document.createElement("OPTION");
                    option.value = item.id_estacion;
                    option.innerText = item.nombre_estacion;
                    $("#estaciones").append(option);
                    
                })
                obtenerParametros()
            })
            .catch(error => console.log(error));
    }

    var obtenerParametros = function () {
        $("#grupo_parametros").empty();
        groupWithParam[0]['parametros'].forEach(item => {
            var option = document.createElement("OPTION");
            option.value = item.id_parametro;
            option.innerText = item.nombre_parametro;
            $("#grupo_parametros").append(option);
        })
    }

    var selectParametros = function () {
        $('#grupo_parametros option').prop('selected', false);
        var posicion = $("#grupos").val()
        var grupoSeleccionado = groupWithParam.find(function (grupo) {
            return grupo.id_grupo == posicion;
        });

        if (grupoSeleccionado) {
            grupoSeleccionado['parametros'].forEach(function (item) {
                $('#grupo_parametros option').filter(function () {
                    return $(this).text() === item.nombre_parametro;
                }).prop('selected', true);
            });
        } else {
            console.log("Grupo no encontrado");
        }
        $("#grupo_parametros").select2()
    }
    var estadoInicialSorteable = [];
    var obtenerAllParametros = function () {
        $.post(urlGetAllParametroTelemetria, null)
            .then(response => {
                response.forEach(item => {
                    var option = document.createElement("OPTION");
                    option.value = item.id;
                    option.innerText = item.nombre_parametro;
                    $("#grupo_parametros").append(option);
                });
                $(".grupo_parametros2").each(function () {
                    var select = $(this);
                    select.empty();

                    select.append($('<option>', {
                        text: 'Seleccionar parámetro',
                        selected: true,
                        disabled: true
                    }));

                    response.forEach(item => {
                        var option = new Option(item.nombre_parametro, item.id);
                        select.append(option);
                    });

                    select.select2({
                        width: '100%'
                    });
                });

                var lista = $('#sortable1');
                lista.empty(); // Limpia los elementos <li> existentes

                // Crea y agrega nuevos <li> a la lista
                response.forEach(item => {
                    var li = $('<li>', {
                        class: 'ui-state-default',
                        id: item.id
                    });
                    var div = $('<div>', {
                        class: 'list-group-item list-group-item-primary',
                        role: 'alert',
                        text: item.nombre_parametro // Asumiendo que quieres mostrar el 'nombre_parametro'
                    });
                    li.append(div);
                    lista.append(li);
                });
                estadoInicialSorteable = $("#sortable1").children().clone();
            })
            .catch(error => console.log(error));
    }
    var limitesWithParam = []
    var obtenerLimite = function () {
        $.post(urlGetAllLimiteTelemetria, null)
            .then(response => {
                limitesWithParam = response
                $("#limites").empty()

                var defaultOption = document.createElement("option");
                defaultOption.disabled = true;
                defaultOption.selected = true;
                defaultOption.innerText = "Seleccionar Limite";
                $("#limites").prepend(defaultOption);

                response.forEach(item => {
                    var option = document.createElement("OPTION");
                    option.value = item.id_limite;
                    option.innerText = item.nombre_limite;
                    $("#limites").append(option);
                });
            })
            .catch(error => console.log(error)); 
    }
    obtenerLimite()
    var groupWithParam = []

    var obtenerAllGrupoParametros = function () {
        $.post(urlGetAllGrupoParametroTelemetria, null)
            .then(response => {
                $("#grupos").empty()

                var defaultOption = document.createElement("option");
                defaultOption.disabled = true;
                defaultOption.selected = true;
                defaultOption.innerText = "Seleccionar grupo";
                $("#grupos").prepend(defaultOption);

                groupWithParam = response
                response.forEach(item => {
                    var option = document.createElement("OPTION");
                    option.value = item.id_grupo;
                    option.innerText = item.nombre_grupo_parametro;
                    $("#grupos").append(option);
                });
            })
            .catch(error => console.log(error));
    }

    var obtenerAllStation = function () {
        $.post(urlGetAllStationTelemetria, null)
            .then(response => {
                response.forEach(item => {
                    var option = document.createElement("OPTION");
                    option.innerText = item.nombre_estacion;
                    $("#estaciones").append(option);
                });
            })
            .catch(error => console.log(error));
    }

    var data = []
    var fecha = []
    var obtenerData = function () {
        model = {
            id_estacion: $("#estaciones").val(),
            id_parametro: $("#parametros").val(),
        }
        $.post(urlGetDataTelemetria, model)
            .then(response => {
                data = []
                fecha = []
                response.forEach(item => {
                    data.push([item.fecha_muestreo, item.resultado])
                    fecha.push(item.fecha_muestreo)
                })
                armarGraficoParametros(fecha, data)
            })
            .catch(error => console.log(error));
    }

    var enviarGrupoParametro = function (data) {
        $.post(urlSetGrupoParametrosTelemetria, data)
            .then(response => {
                toastr.success(decodeURIComponent(JSON.parse('"' + response.replace(/\"/g, '\\"') + '"')));
                obtenerAllGrupoParametros()
                $('#modalCrear').modal('hide')
            })
            .catch(error => console.log(error)); 
    }

    $("#editar").on('click', function () {
        $("#sortable2").empty();
        $("#sortable1").html(estadoInicialSorteable.clone());
        $("#sortable1").sortable();        
        var selectedOption = $('#grupos option:selected');
        var grupoId = selectedOption.val(); // Obtiene el valor (ID)
        var nombre_grupo = selectedOption.text();
        
        var grupoBuscado = groupWithParam.find(grupo => grupo.id_grupo == grupoId);
        var idsParametros = grupoBuscado.parametros.map(parametro => parametro.id_parametro);
        seleccionSorteable(idsParametros)
        $('#nombre_grupo').val(nombre_grupo)
        $('#modalCrear').modal('show')
    })

    $("#crear").on('click', function () {
        $("#sortable2").empty();
        $("#sortable1").html(estadoInicialSorteable.clone());
        $("#sortable1").sortable();   
        $('#modalCrear').modal('show')
    })

    $("#editarLimite").on('click', function () {
        
        var selectedOption = $('#limites option:selected');
        var limiteId = selectedOption.val(); // Obtiene el valor (ID)
        var nombre_limite = selectedOption.text();

        var grupoBuscado = limitesWithParam.find(limite => limite.id_limite == limiteId);
        //var idsParametros = grupoBuscado.parametros.map(parametro => parametro.id_parametro);
        $('#nombre_limite').val(nombre_limite)

        var parameterRows = $("#rowsContainer .row").slice(2);
        parameterRows.each(function (index, row) {
            if (index < grupoBuscado.parametros.length) {
                var parametro = grupoBuscado.parametros[index];
                $(row).find('.grupo_parametros2').val(parametro.parametro_id).trigger('change');;
                $(row).find('.limite_inferior').val(parametro.limite_inferior);
                $(row).find('.limite_superior').val(parametro.limite_superior);
            }
        });
        $('#crearLimiteModal').modal('show')
    })

    $("#guardar_limite").on('click', function () {
        var selectedOption = $('#limites option:selected');
        var limiteId = selectedOption.val(); // Obtiene el valor (ID)
        if (limiteId != null) {
            uploadLimite(limiteId);
        } else {
            uploadLimite();
        }
        
    })

    $("#guardar_grupo").on('click', function () {
        var nombre_grupo = $("#nombre_grupo").val()
        var id_grupo_parametro = $("#grupos").val()
        if (id_grupo_parametro == null) {
            id_grupo_parametro = 0
        }
        var parametros = [];
        $("#sortable2 li").each(function () {
            var parametroId = parseInt($(this).attr('id')); // Asumiendo que el id del li es el parametro_id
            parametros.push({ "parametro_id": parametroId });
        });
        var data = {
            "id_grupo_parametro": id_grupo_parametro,
            "nombre_grupo_parametro": nombre_grupo,
            "parametros": parametros
        };
        enviarGrupoParametro(data);
        //Limpio los parametros
        $('#grupo_parametros').val(null).trigger('change');
    })
    var seleccionSorteable = function (ids) {
        var selectedIds = ids
        selectedIds.forEach(function (id) {
            $('#' + id).appendTo('#sortable2');
        });
    }
  return {
    init: iniciar
  };
}();

$(document).ready(function () {
    $("#grupo_parametros").select2()
    $("#estaciones").select2()
    $("#crearLimite").on('click', function () {
        $('#crearLimiteModal').modal('show')
    })
 
    $("#sortable1, #sortable2").sortable({
        connectWith: ".connectedSortable",
        stop: function (event, ui) {
            // Si el elemento se soltó en #sortable2, registra los IDs de todos los elementos en #sortable2
            if (ui.item.parent().attr("id") === "sortable2") {
                itemsInSortable2 = $("#sortable2 li").map(function () {
                    return $(this).attr("id");
                }).get();
            }
        }
    }).disableSelection();

    var generarFilas = function () {
        var totalRows = 100;  // Total de filas que deseas generar
        var visibleRows = 9;  // Número de filas que serán visibles inicialmente
        var template = document.getElementById('rowTemplate');
        var container = document.getElementById('rowsContainer');

        for (let i = 0; i < totalRows; i++) {
            var clone = template.cloneNode(true); // Clona el template
            clone.id = ""; // Elimina el id para evitar ids duplicados

            // Configura la visibilidad de la fila según su índice
            clone.style.display = (i >= visibleRows) ? "none" : "";

            // Elimina el input oculto existente
            var existingHiddenInput = clone.querySelector('.row-index');
            if (existingHiddenInput) {
                existingHiddenInput.parentNode.removeChild(existingHiddenInput);
            }

            // Agrega un input oculto para la posición
            var input = document.createElement('input');
            input.type = 'hidden';
            input.className = 'row-index';
            input.value = i+2;
            clone.appendChild(input); // Añade el input oculto al clon

            container.appendChild(clone); // Añade la fila clonada al contenedor
        }
    }

    generarFilas();

    var showNextRowButton = document.getElementById('showNextRowButton');

    showNextRowButton.addEventListener('click', function () {
        // Encuentra la primera fila oculta y muestra la siguiente
        var hiddenRows = document.querySelectorAll('#rowsContainer .row[style*="display: none"]');
        if (hiddenRows.length > 0) {
            hiddenRows[0].style.display = ''; // Muestra la primera fila oculta
        } else {
            alert('No hay más filas para mostrar.'); // Opcional: mensaje cuando no hay más filas ocultas
        }
    });
    detalleMuestras.init();    
});
