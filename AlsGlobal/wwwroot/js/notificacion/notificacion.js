var notificacion = function () {
    var iniciar = function () {
        getAllNotificaciones()
    }

    var notificaciones = []
    var getAllNotificaciones = function () {
        $.post(urlGetNotificaciones, null)
            .then(response => {
                notificaciones = response
                const container = document.querySelector('.lista_notificaciones');
                notificaciones.forEach(notificacion => {
                    const alertClass = notificacion.nivel_notificacion_id === 2 ? 'text-warning' : 'text-peligro';
                    const iconClass = notificacion.nivel_notificacion_id === 2 ? 'fa-exclamation-circle' : 'fa-exclamation-triangle';
                    const date = new Date(notificacion.created_at);
                    const formattedTime = date.toLocaleTimeString([], {hour: '2-digit', minute: '2-digit', hour12: true, timeZone: 'America/New_York'});
                    console.log(notificacion.created_at)
                    const notificationElement = document.createElement('div');
                    notificationElement.className = 'notification d-flex align-items-center alert-item';
                    notificationElement.setAttribute('data-id', notificacion.id);

                    const iconElement = document.createElement('i');
                    iconElement.className = `fas ${iconClass} fa-2x ${alertClass}`;
                    notificationElement.appendChild(iconElement);

                    const textContainer = document.createElement('div');
                    textContainer.className = 'ms-3 flex-grow-1 ml-3';

                    const titleElement = document.createElement('div');
                    titleElement.className = 'fw-bold';
                    titleElement.textContent = notificacion.titulo;
                    textContainer.appendChild(titleElement);

                    const descriptionElement = document.createElement('div');
                    descriptionElement.className = 'text-muted';
                    descriptionElement.textContent = notificacion.descripcion;
                    textContainer.appendChild(descriptionElement);

                    notificationElement.appendChild(textContainer);

                    const timeElement = document.createElement('div');
                    timeElement.className = 'notification-time ms-auto';
                    timeElement.textContent = formattedTime; // Usando el tiempo formateado
                    notificationElement.appendChild(timeElement);

                    container.appendChild(notificationElement);
                });

                document.querySelectorAll('.alert-item').forEach(item => {
                    item.addEventListener('click', function () {
                        var info = this.getAttribute('data-id'); // Extrae la información almacenada
                        showModal(info);
                    });
                });
            })
            .catch(error => console.log(error));
    }

    var showModal = function (notificacionId) {

        const notificacion = notificaciones.find(n => n.id === parseInt(notificacionId));

        //document.getElementById('modalBody').textContent = info; // Actualiza el cuerpo del modal
        if (notificacion) {
            const infoAdicional = JSON.parse(notificacion.informacion_adicional);
            const modalBody = document.getElementById('modalBody');
            modalBody.innerHTML = ''; // Limpiar contenido anterior

            Object.keys(infoAdicional).forEach(key => {
                const infoRow = document.createElement('div');
                infoRow.className = 'row mb-2'; // Añadiendo un margen
                infoRow.innerHTML = `<div class="col-sm-4 font-weight-bold">${key}:</div><div class="col-sm-8">${infoAdicional[key]}</div>`;
                modalBody.appendChild(infoRow);
            });

            $('#infoModal').modal('show'); // Muestra el modal
        }
        
    }
    return {
        init: iniciar
    };
}();

$(document).ready(function () {
    notificacion.init();
});