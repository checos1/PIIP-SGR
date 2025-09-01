'use strict';

/**
 * @ngdoc directive
 * @name hasClaim
 * @restrict A
 * @multiElement
 *
 * @description
 * La directiva `hasClaim` elimina o recrea una parte del árbol DOM basándose en {[matriz]}. 
 * Si los valores asignados a `hasClaim` se evalúan como un valor falso, entonces el elemento 
 * se elimina del DOM; de lo contrario, se vuelve a insertar el elemento en el DOM.
 *
 * @element ANY
 * @scope hasClaim
 * @priority 600
 * @param {has-menus, has-entidades, has-opiciones} hasClaim la directiva recibirá tres tipos de 
 * parámetros opcionales, si el usuario que ha iniciado sesión tiene la información, el elemento 
 * se mostrará. Cuando se les informa más de un parámetro, funcionarán como una agregación 
 * de condicionales.
 *
 * @example
  <example has-claim
    has-entidades="['23200d-dsd4343-dsf343-343534asdafv']"
    has-menus="['MenuItem', 'MenuItem2', 'MenuItem3']"
    has-opciones="['MenuItem:Crear', 'MenuItem:Actualizar']">
    ....
  </example>
 */
angular.module('backbone.core.directives')
    .directive('hasClaim', ['$animate', '$compile', 'sesionServicios', '$sessionStorage', function ($animate, $compile, sesionServicios, $sessionStorage) {
        return {
            scope: {
                menus: '=?hasMenus',
                entidades: '=?hasEntidades',
                opciones: '=?hasOpciones'
            },
            multiElement: true,
            transclude: 'element',
            priority: 600,
            terminal: true,
            restrict: 'A',
            $$tlb: true,
            link: function($scope, $element, $attr, ctrl, $transclude) {
                try {                 
                    const opciones = Array.isArray($scope.opciones) && $scope.opciones || undefined;
                    var entidades = Array.isArray($scope.entidades) && $scope.entidades || undefined;
                    const menus = Array.isArray($scope.menus) && $scope.menus || undefined;

                    //console.log('opciones: ', opciones)
                    //console.log('entidades: ', entidades)
                    //console.log('menus: ', menus)


                    if (entidades) {
                        var upperEntidades = [];
                        entidades.forEach(entidad => upperEntidades.push(typeof entidad === 'string' ? entidad.toUpperCase() : ''));
                        entidades = upperEntidades;
                    }

                    const permisos = sesionServicios.obtenerPermisos()

                  //  console.log('permisos: ', permisos)

                    permisos.Entidades.forEach(e => e.IdEntidad = typeof e.IdEntidad === 'string' ? e.IdEntidad.toUpperCase() : '');

                    const tieneEntidades = _obtenerEntidades(permisos, entidades);
                    const tieneMenus = _obtenerMenus(permisos, menus);
                    const tieneOpciones = _obtenerOpciones(permisos, opciones, entidades);

                    if (!$sessionStorage.soloLectura) {
                        if ((tieneEntidades && tieneMenus && tieneOpciones))
                            $transclude((clone, newScope) => $animate.enter(clone, $element.parent(), $element));
                    }
                    

                } catch (error) {
                    throw `Parámetros no informados correctamente: ${error}`
                }

                function _obtenerEntidades(permisos, entidades) {
                    return permisos.Entidades.some(entidad => !entidades || upperEntidades.includes(entidad.IdEntidad));
                }

                function _obtenerMenus(permisos, menus) {
                    return permisos.OpcionesMenu.some(menu => !menus || menus.includes(menu));
                }

                function _obtenerOpciones(permisos, opciones, entidades) {
                    return permisos.Entidades
                        .filter(entidad => !entidades || entidades.includes(entidad.IdEntidad))
                        .some(entidad => {
                            return entidad.Opciones.some(opcion => !opciones || opciones.includes(opcion.IdOpcionDNP));
                    })
                }
            }
        };
    }]);