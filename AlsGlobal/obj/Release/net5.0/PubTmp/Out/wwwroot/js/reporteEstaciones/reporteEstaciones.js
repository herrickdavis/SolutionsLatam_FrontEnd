var reporteEstaciones = function () {
  var parametroSeleccionado = "";
  var mapa = null;
  var infoAbiertos = false;
  var markers = [];
  var iniciar = function () {
    inicializarEvento();
  }
  var inicializarEvento = function () {
    $("#id_matriz").on("change", obtenerParametros);
    $("#id_parametros").on("change", obtenerEstaciones);
    $("#btnBuscar").on("click", buscarEstaciones);
  }
  var obtenerParametros = function (ev, picker) {
    var model = {
      id_tipo_muestra: $(this).val()
    }
    $.post(urlGetParametrosReporteEstaciones, model, "#divParametro,#divEstacion")
      .then(response => {
        $("#id_parametros, #id_estacion").empty();
        response.forEach(x => {
          var option = document.createElement("OPTION");
          option.value = x.value;
          option.innerText = x.text;
          $("#id_parametros").append(option);
        });
        $('#id_parametros').selectpicker('refresh');
      });
  }
  var obtenerEstaciones = function (ev, picker) {
    var model = {
      id_tipo_muestra: $("#id_matriz").val(),
      id_parametro: $("#id_parametros").val()
    }
    $.post(urlGetEstacionesReporteEstaciones, model, "#divEstacion")
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
  var buscarEstaciones = function () {

    var model = {
      id_tipo_muestra: $("#id_matriz").val(),
      id_parametro: $("#id_parametros").val(),
      id_estaciones: $("#id_estacion").val()
    }
    $.post(urlGetReporteEstaciones, model, "card-body:first, #divDetalle")
      .then(response => {
        parametroSeleccionado = $("#id_parametros option:selected").text();
        $("#divDetalle").empty();
        $("#divDetalle").html(response);
        $("#lblParametro").html(parametroSeleccionado);
        armarMapa();
        $("#btnEtiquetas").on("click", mostrarOcultarEtiquetas);
      });
  };
  var armarMapa = function () {
    markers = [];
    infoAbiertos = false;
    var items = $(".item");
    var tieneLatitud = false;
    var latitud;
    var longitud;
    $.each(items, (index,item) => {
      latitud = item.getAttribute("data-latitud");
      if (latitud !== null && latitud.trim().length > 0) {
        longitud = item.getAttribute("data-longitud");
        tieneLatitud = true;
      }
    })

    if (tieneLatitud) {
      $("#divDetalle .card-body:first").show("fast");
      mapa = new GMaps({
        div: '#divMapa',
        lat: latitud,
        lng: longitud,
        zoomControl: true,
        mapTypeControl: true,
        scaleControl: true,
        streetViewControl: true,
        rotateControl: true,
        fullscreenControl: true,
        draggable: true,
        zoom: 12,
        mapTypeId: 'satellite'
      });

      $.each(items, (index, item) => {
        latitud = item.getAttribute("data-latitud");
        longitud = item.getAttribute("data-longitud");
        if (latitud !== null && latitud.trim().length > 0) {
          var marker = new google.maps.Marker({
            position: new google.maps.LatLng(latitud, longitud)
          });
          marker.setMap(mapa.map);
          markers.push({ fila: item, marker: marker });
        }
      })
    }
    else {
      $("#divDetalle .card-body:first").hide("fast");
    }
  }
  var mostrarOcultarEtiquetas = function () {
    if (infoAbiertos === true) {
      infoAbiertos = false;
      $("#btnEtiquetas").html(verEtiquetas);
      $.each(markers, (index) => {
        markers[index].infoWindow.close();
      });
    } else {
      infoAbiertos = true;
      $.each(markers, (index) => {
        var infowindow = new google.maps.InfoWindow({
          content: `<div style='text-align:center'>
                                    <b>${markers[index].fila.getAttribute("data-estacion")}</b><br>
                                    ${parametroSeleccionado}:  ${markers[index].fila.getAttribute("data-valor")}
                                </div>`,
          maxWidth: 160
        });
        markers[index].infoWindow = infowindow;
        infowindow.open(mapa, markers[index].marker);
      });
      $("#btnEtiquetas").html(ocultarEtiquetas);
    }
  }
  return {
    init: iniciar
  };
}();
jQuery(document).ready(function () {
  reporteEstaciones.init();
});
