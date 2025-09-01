(function () {
    'use strict';

    angular.module('backbone').factory('firmaFormularioSgrServicio', firmaFormularioSgrServicio);

    firmaFormularioSgrServicio.$inject = ['$http', 'constantesBackbone'];

    function firmaFormularioSgrServicio($http, constantesBackbone) {

        return {
            cargarFirma: cargarFirma,
            borrarFirma: borrarFirma,
            firmar: firmar,
            validarSiExisteFirmaUsuario: validarSiExisteFirmaUsuario
        };

        function cargarFirma(firma, rol, usuario) {
            const parametro = {
                FileAsBase64: firma,
                RolId: rol,
                UsuarioId: usuario
            }
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneCargarFirmaSgr}`;
            return $http.post(url, parametro);
        }

        function borrarFirma(rol, usuario) {
            const parametro = {
                RolId: rol,
                UsuarioId: usuario
            }
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneBorrarFirmaSgr}`;
            return $http.post(url, parametro);
        }

        function firmar(instanciaId, tipoConceptoViabilidadId, usuarioDnp, entidadId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneFirmarSgr}?instanciaId=${instanciaId}&tipoConceptoViabilidadId=${tipoConceptoViabilidadId}&usuarioDnp=${usuarioDnp}&entidadId=${entidadId}`;
            return $http.post(url);

        }

        function validarSiExisteFirmaUsuario(usuarioDnp) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneValidarSiExisteFirmaUsuarioSgr}?usuarioDnp=${usuarioDnp}`;
            return $http.post(url);
        }
    }
})();