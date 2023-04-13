var detalleMuestras = function () {
  var iniciar = function () {
    inicializarEvento();
    inicializarMapa();
  }
  var inicializarEvento = function () {
    $("#ddlValoresMuestra").on("change", obtenerParametros);
    $("#btnFiltrar").on("click", ocultarParametros);
    var card = new KTCard('cardMapa');
  }
  var obtenerParametros = function () {
    var id_muestra = $(this).attr("data-id");
    var id_limite = $(this).val();
    $.get(`${urlObtenerParametros}?id_muestra=${id_muestra}&id_limite=${id_limite}`, "#table-parametros")
      .then(function (html) {
        $("#table-parametros").empty();
        $("#table-parametros").html(html);
      });
  }
  var ocultarParametros = function () {
    var attr = $(this).attr("data-ocultado");
    if (attr === "true") {
      $(this).attr("data-ocultado", false);
      $(".table tbody tr[data-mostrar=False]").show("fast");
    }
    else {
      $(this).attr("data-ocultado", true);
      $(".table tbody tr[data-mostrar=False]").hide("fast");
    }
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
  return {
    init: iniciar
  };
}();
