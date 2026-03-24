// Modelo de una tarea
function TareaElemento({ id, titulo, descripcion }) {
    var self = this;
    self.id = ko.observable(id);
    self.titulo = ko.observable(titulo);
    self.descripcion = ko.observable(descripcion || ""); // <- aquí
    self.esNuevo = ko.pureComputed(function () {
        return self.id() === 0;
    });
}


function tareaElementoListadoViewModel({ id, titulo, descripcion }) {
    var self = this;

    self.id = ko.observable(id);
    self.titulo = ko.observable(titulo);
    self.descripcion = ko.observable(descripcion || "");

    self.esNuevo = ko.pureComputed(function () {
        return self.id() === 0;
    });
}

// ViewModel principal
function TareaListadoViewModel() {
    var self = this;

    self.tareas = ko.observableArray([]);
    self.tareaSeleccionada = ko.observable(null);
    self.cargando = ko.observable(false);
    self.noHayTareas = ko.observable(false);

    // Abrir modal de edición
    self.abrirModalEditar = function (tarea) {
        self.tareaSeleccionada(tarea); // asigna la tarea al observable
        modalEditarTareaBootstrap.show();
    };

    // Manejar focusout de nueva tarea
    self.manejarFocusoutTituloTarea = function (tarea, event) {
        if (!tarea.titulo() || tarea.titulo().trim() === '') {
            if (tarea.esNuevo()) {
                self.tareas.remove(tarea);
            }
        } else {
            console.log("Tarea lista para guardar:", tarea.titulo());
        }
    };

    // Agregar nueva tarea
    self.agregarNuevaTarea = function () {
        const nuevaTarea = new TareaElemento({ id: 0, titulo: '', descripcion:'' });
        self.tareas.push(nuevaTarea);

        setTimeout(() => {
            const input = document.querySelector("input[name='titulo-tarea']:last-child");
            if (input) input.focus();
        }, 100);
    };

    // Guardar cambios en modal al controlador
    self.guardarTarea = function () {
        const tarea = self.tareaSeleccionada();
        if (!tarea) return;

        const datos = {
            id: tarea.id(),
            titulo: tarea.titulo(),
            descripcion: tarea.descripcion()
        };
        
        fetch("/api/tareas", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]')?.value
            },
            body: JSON.stringify(datos)
        })

            .then(response => {
                if (!response.ok) throw new Error("Error al guardar tarea");
                return response.json();
            })
            .then(resultado => {
                if (resultado.exito) {
                    // Actualizar id en caso de nueva tarea
                    tarea.id(resultado.id);
                    alert("Tarea guardada correctamente");
                    obtenerTareas();  
                    modalEditarTareaBootstrap.hide();
                } else {
                    alert("Error: " + resultado.mensaje);
                }
            })
            .catch(error => {
                console.error(error);
                alert("Ocurrió un error al guardar la tarea");
            });
    };
    self.eliminarTarea = function () {
        const tarea = self.tareaSeleccionada();
        if (!tarea) return;

        fetch(`/api/tareas/${tarea.id()}`, {
            method: "DELETE",
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]')?.value
            }
        })
            .then(response => {
                if (!response.ok) throw new Error("Error al borrar tarea");
                return response.json();
            })
            .then(resultado => {
                if (resultado.exito) {
                    alert("Tarea eliminada correctamente");
                    obtenerTareas();  
                    modalEditarTareaBootstrap.hide();
                } else {
                    alert("Error: " + resultado.mensaje);
                }
            })
            .catch(error => {
                console.error(error);
                alert("Ocurrió un error al eliminar la tarea");
            });
    };

}

// Instanciamos el ViewModel
const tareaListadoViewModel = new TareaListadoViewModel();

// Función global para agregar nueva tarea desde botón
function agregarNuevaTarea() {
    tareaListadoViewModel.agregarNuevaTarea();
}

// ===============================
// Obtener tareas
// ===============================
async function obtenerTareas() {
    tareaListadoViewModel.cargando(true);

    const respuesta = await fetch(urlTareas, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    });

    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
         tareaListadoViewModel.cargando(false);
        return;
    }

    const json = await respuesta.json();

    // ✅ CORRECTO
    tareaListadoViewModel.tareas([]);

    json.forEach(tarea => {
        tareaListadoViewModel.tareas.push(
            new tareaElementoListadoViewModel(tarea)
        );
    });

    tareaListadoViewModel.cargando(false);
}


