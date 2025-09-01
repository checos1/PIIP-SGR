/*************************************************************
** Nombre: observacionModalController
** Descripción:
** Controlador del modal de observación que permite realizar
** operaciones relacionadas con la gestión de subpasos en 
** flujos de trabajo. Integra notificaciones y redirecciones.

** Autor: Gonzalo Andrés Lucio López
** Fecha: 2025-01-22

** Historial de Cambios:
**************************************************************
** Ítem   Fecha       Autor                 Descripción
** 01     2025-01-22  Gonzalo Lucio         Creación inicial.
*************************************************************/

// Definición del controlador del modal de observación
angular.module('backbone').controller('observacionModalController', observacionModalController);

// Inyección de dependencias
observacionModalController.$inject = [
    '$uibModalInstance',
    '$location',
    'avance', 'instancia', 'accion', 'mensajeNotificacion', 'mensajeExito',
    'flujoServicios', 'utilidades'
];

/**
 * Controlador principal del modal de observación.
 * @param {Object} $uibModalInstance - Instancia del modal para controlarlo.
 * @param {number} avance - Identificador del avance (paso/subpaso).
 * @param {string} instancia - ID de la instancia en ejecución.
 * @param {string} accion - ID de la acción asociada.
 * @param {string} mensajeNotificacion - Mensaje de notificación para el usuario.
 * @param {string} mensajeExito - Mensaje de éxito tras completar el subpaso.
 * @param {Object} flujoServicios - Servicio para ejecutar operaciones en el flujo.
 * @param {Object} utilidades - Servicio de utilidades para manejo de mensajes y redirecciones.
 */
function observacionModalController(
    $uibModalInstance,
    $location,
    avance, instancia, accion, mensajeNotificacion, mensajeExito,
    flujoServicios, utilidades
) {
    var vm = this;

    // Inicializa las variables del controlador
    vm.avance = avance; // Representa el nivel de avance (siguiente - devolución - paso o subpaso)
    vm.instancia = instancia; // ID de la instancia del flujo
    vm.accion = accion; // ID de la acción en ejecución
    vm.mensajeExito = mensajeExito; // Mensaje que se muestra al completar
    vm.mensajeNotificacion = mensajeNotificacion; // Mensaje inicial para el modal
    vm.redirectIndexPage = redirectIndexPage; // Función Redirección a la página de inicio
    vm.redirectConsolaProcesos = redirectConsolaProcesos; // Función Redirección a la consola de procesos

    /**
     * Función para aceptar y ejecutar el subpaso actual.
     */

    vm.aceptar = function () {
        EjecutarSubPaso();
    };

    /**
     * Función para cancelar el modal.
     */
    vm.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };

    /**
     * Ejecuta el subpaso en el flujo de trabajo.
     * Envía datos al servidor a través de flujoServicios.Flujos_SubPasoEjecutar.
     */
    function EjecutarSubPaso() {
        var data = {
            InstanciaId: vm.instancia,
            idAccion: vm.accion,
            AvanceId: vm.avance,
            Observacion: vm.observacion // Observación proporcionada por el usuario
        };

        return flujoServicios.Flujos_SubPasoEjecutar(data)
            .then(function (response) {
                if (response.data && response.status === 200) {
                    $uibModalInstance.close(vm.observacion); // Cierra el modal con éxito
                    utilidades.mensajeSuccess(
                        "Ahora usted puede acceder a este proceso desde la consola de procesos.",
                        true,
                        vm.redirectIndexPage,
                        vm.redirectConsolaProcesos,
                        mensajeExito,
                        "IR A MIS PROCESOS",
                        "IR CONSOLA DE PROCESOS"
                    );
                    return;
                }
            })
            .catch(error => {
                if (error.status == 400) {
                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                    $uibModalInstance.dismiss('cancel');
                    return;
                }
                utilidades.mensajeError("Error al realizar la operación");
            });
    }
    /**
     * Redirige a la página de inicio después de completar el subpaso.
     */
    function redirectIndexPage() {
        location.reload();
    }

    /**
     * Redirige a la consola de procesos después de completar el subpaso.
     */
    function redirectConsolaProcesos() {
        $location.url("/consolaprocesos/index");
    }
}
