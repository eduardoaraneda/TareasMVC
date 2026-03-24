async function manejarErrorApi(response) {
    let mensaje = 'Ocurrió un error inesperado.';
    if (response && response.status) {
        if (response.status === 400) {
            const data = await response.json();
            if (data && data.error) {
                mensaje = data.error;
            } else {
                mensaje = 'Solicitud incorrecta.';
            }
        } else if (response.status === 404) {
            mensaje = 'Recurso no encontrado.';
        } else if (response.status === 500) {
            mensaje = 'Error interno del servidor.';
        } else {
            mensaje = `Error: ${response.status} ${response.statusText}`;
        }
    }
    mostrarMensaje(mensaje);
}

function mostrarMensaje(mensaje) {  
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: mensaje
    });
}
