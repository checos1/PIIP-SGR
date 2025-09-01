(function () {
    'use strict';

    angular.module('backbone').factory('firmaFormularioSgpServicio', firmaFormularioSgpServicio);

    firmaFormularioSgpServicio.$inject = ['$http', 'constantesBackbone'];

    function firmaFormularioSgpServicio($http, constantesBackbone) {

        return {
            cargarFirma: cargarFirma,
            borrarFirma: borrarFirma,
            firmar: firmar,
            validarSiExisteFirmaUsuario: validarSiExisteFirmaUsuario,
            eliminarFirma: eliminarFirma
        };

        function cargarFirma(firma, rol, usuario) {
            const parametro = {
                FileAsBase64: firma,
                RolId: rol,
                UsuarioId: usuario
            }
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneCargarFirmaSgp}`;
            return $http.post(url, parametro);
        }

        function borrarFirma(rol, usuario) {
            const parametro = {
                RolId: rol,
                UsuarioId: usuario
            }
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneBorrarFirmaSgp}`;
            return $http.post(url, parametro);
        }

        function firmar(instanciaId, tipoConceptoViabilidadId, usuarioDnp, entidadId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneFirmarSgp}?instanciaId=${instanciaId}&tipoConceptoViabilidadId=${tipoConceptoViabilidadId}&usuarioDnp=${usuarioDnp}&entidadId=${entidadId}`;
            return $http.post(url);

        }

        function eliminarFirma(instanciaId, tipoConceptoViabilidadId, usuarioDnp, entidadId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneEliminarFirmaSgp}?instanciaId=${instanciaId}&tipoConceptoViabilidadId=${tipoConceptoViabilidadId}&usuarioDnp=${usuarioDnp}&entidadId=${entidadId}`;
            return $http.post(url);

        }

        function validarSiExisteFirmaUsuario(usuarioDnp) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneValidarSiExisteFirmaUsuarioSgp}?usuarioDnp=${usuarioDnp}`;
            return $http.post(url);
        }
    }
})();