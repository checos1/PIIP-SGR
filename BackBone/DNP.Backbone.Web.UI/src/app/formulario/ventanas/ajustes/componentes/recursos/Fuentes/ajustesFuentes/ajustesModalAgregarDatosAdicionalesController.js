(function () {
    'use strict';

    ajustesModalAgregarDatosAdicionalesController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'recursosAjustesServicio',
        'utilidades'
    ];

    function ajustesModalAgregarDatosAdicionalesController(
        $uibModalInstance,
        $sessionStorage,
        recursosAjustesServicio,
        utilidades
    ) {
        var vm = this;
        vm.init = init;
        vm.cerrar = $sessionStorage.close;
        vm.cambioTipo = cambioTipo;
        vm.cambioFondo = cambioFondo;
        vm.cambioRubro = cambioRubro;
        vm.buscarProyecto = buscarProyecto;
        vm.adicionarDato = adicionarDato;
        vm.guardar = guardar;
        vm.eliminarDatoAdicional = eliminarDatoAdicional;

        vm.fondoVisible = false;
        vm.proyectoVisible = false;
        vm.rubroVisible = false;
        vm.tblAdicionarDato = false;

        vm.BPIN = $uibModalInstance.idObjetoNegocio;
        vm.idFuente = $uibModalInstance.idFuente;
        vm.proyectoId = $uibModalInstance.proyectoId
        vm.proyectoIdBpin = 0;
        vm.idRubroSeleccionado = 0;

        var lstRolesTodo = $uibModalInstance.usuario.roles;
        var lsRoles = [];
        for (var ls = 0; ls < lstRolesTodo.length; ls++)
            lsRoles.push(lstRolesTodo[ls].IdRol)

        var parametros = {
            "Aplicacion": nombreAplicacionBackbone,
            "ListaIdsRoles": lsRoles,
            "IdUsuario": usuarioDNP,
            "IdObjeto": $uibModalInstance.idInstancia,      //'88ea329d-f240-4868-9df7-86c74fb2ecfa',
            "InstanciaId": $uibModalInstance.idInstancia,   //'88ea329d-f240-4868-9df7-86c74fb2ecfa', 
            "IdFiltro": $uibModalInstance.idAccionAnterior
        }

        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            consultarDatos();
        }

        function consultarDatos() {

            var listaDatosAdicionales = [];
            return recursosAjustesServicio.obtenerDatosAdicionales(vm.idFuente, usuarioDNP, $uibModalInstance.idInstancia)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arreglolistaDatos = jQuery.parseJSON(respuesta.data);
                    var arregloDatos = jQuery.parseJSON(arreglolistaDatos);

                    for (var ls = 0; ls < arregloDatos.length; ls++) {
                        if (arregloDatos[ls].cofinanciadorId > 0) {
                            var detalleDatos = {
                                "cofinanciadorId": arregloDatos[ls].cofinanciadorId,
                                "tipoCofinanciador": arregloDatos[ls].tipoCofinanciador,
                                "codigoCofinanciador": arregloDatos[ls].codigoCofinanciador,
                                "codigo": arregloDatos[ls].codigo
                            }
                            listaDatosAdicionales.push(detalleDatos);
                        }
                    }
                    vm.listaDatosAdicionales = listaDatosAdicionales;
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar los datos adicionales de la fuente de financiacion");
                });
        }

        function obtenerTipoCofinanciador() {
            var listaTipoCofinanciador = [];

            return recursosAjustesServicio.obtenerTipoCofinanciador(parametros)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var arregloTipoCofinanciador = jQuery.parseJSON(respuesta.data);
                    for (var ls = 0; ls < arregloTipoCofinanciador.length; ls++) {
                        if (arregloTipoCofinanciador[ls].Name != "Privado") {
                            var tipoC = {
                                "Name": arregloTipoCofinanciador[ls].Name,
                                "Id": arregloTipoCofinanciador[ls].Id,
                            }
                            listaTipoCofinanciador.push(tipoC);
                        }
                    }
                    vm.listaTipoCofinanciador = listaTipoCofinanciador;

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar la lista TipoCofinanciador");
                });

        }

        function cambioTipo() {

            textNombre.value = "";

            if (!vm.model.idtipoCofinanciador)
                return;

            switch (vm.model.idtipoCofinanciador) {
                case 1:
                    obtenerProyectos();
                    break;
                case 2:
                    obtenerRubro();
                    break;
                case 3:
                    obtenerFondo();
                    break;
                case 4:
                    vm.fondoVisible = false;
                    vm.rubroVisible = false;
                    vm.proyectoVisible = false;
                    vm.privadoVisible = true;
                    break;
                default:
                    break;
            }
        }

        function obtenerFondo() {
            var listaFondos = [];
            return recursosAjustesServicio.obtenerFondos(parametros)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    vm.rubroVisible = false;
                    vm.proyectoVisible = false;
                    vm.privadoVisible = false;
                    vm.fondoVisible = true;

                    var arregloFondos = jQuery.parseJSON(respuesta.data);
                    for (var ls = 0; ls < arregloFondos.length; ls++) {
                        //
                        var tipoC = {
                            "Name": arregloFondos[ls].Name,
                            "Id": arregloFondos[ls].Id,
                            "Code": arregloFondos[ls].Code + "-" + arregloFondos[ls].Name,
                        }
                        listaFondos.push(tipoC);
                        //}
                    }
                    vm.listaFondos = listaFondos;
                    vm.arreglolistaFondo = listaFondos;
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar la lista Fondos");
                });
        }

        function obtenerRubro() {
            var listaRubro = [];
            return recursosAjustesServicio.obtenerRubros(parametros)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;
                    vm.fondoVisible = false;
                    vm.proyectoVisible = false;
                    vm.privadoVisible = false;
                    vm.rubroVisible = true;

                    var arregloRubro = jQuery.parseJSON(respuesta.data);
                    for (var ls = 0; ls < arregloRubro.length; ls++) {
                        var rubros = {
                            "Name": arregloRubro[ls].Name,
                            "Id": arregloRubro[ls].Name,
                            "Bpin": arregloRubro[ls].Bpin,
                            "Rubro": arregloRubro[ls].Rubro,
                        }
                        listaRubro.push(rubros);
                    }
                    vm.listaRubro = listaRubro;
                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar la lista Rubro");
                });
        }

        function obtenerProyectos() {
            vm.fondoVisible = false;
            vm.rubroVisible = false;
            vm.privadoVisible = false;
            vm.proyectoVisible = true;
        }

        function cambioFondo() {
            var idFondo = vm.model.idFondo;
            if (idFondo == null)
                return

            for (var ls = 0; ls <= vm.listaFondos.length; ls++) {
                if (idFondo == vm.listaFondos[ls].Id) {
                    textNombre.value = vm.listaFondos[ls].Name;
                    vm.model.Nombre = vm.listaFondos[ls].Name;
                    break
                }
            }

        }

        function cambioRubro() {
            var idRubro = vm.model.idRubro;
            if (idRubro == null)
                return

            for (var ls = 0; ls <= vm.listaRubro.length; ls++) {
                if (idRubro == vm.listaRubro[ls].Rubro) {
                    textNombre.value = vm.listaRubro[ls].Name;
                    vm.idRubroSeleccionado = vm.listaRubro[ls].Id;
                    break
                }
            }
        }

        function buscarProyecto() {
            var listaProyectos = [];

            if (vm.model.BpinBusqueda == null || vm.model.BpinBusqueda == "") {
                utilidades.mensajeError("Debe ingresar un Bpin para la busqueda de proyectos.", false);
                return false;
            }

            return recursosAjustesServicio.obtenerProyectos(parametros, vm.model.BpinBusqueda)
                .then(respuesta => {
                    if (!respuesta.data) {
                        utilidades.mensajeError("Bpin no existe, verifiquelo por favor.", false);
                        return;

                    }
                    var arregloProyectos = jQuery.parseJSON(respuesta.data);
                    for (var ls = 0; ls < arregloProyectos.length; ls++) {
                        var proyectos = {
                            "ProyectoNombre": arregloProyectos[ls].ProyectoNombre,
                            "ProyectoId": arregloProyectos[ls].ProyectoId,
                        }
                        listaProyectos.push(proyectos);
                        textNombre.value = arregloProyectos[ls].ProyectoNombre;
                        vm.proyectoIdBpin = arregloProyectos[ls].ProyectoId;
                    }
                    vm.listaProyectos = listaProyectos;

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar la lista de proyectos");
                });
        }

        function adicionarDato() {
            obtenerTipoCofinanciador();
            vm.tblAdicionarDato = true;
        }

        function guardar() {

            //if (vm.tblAdicionarDato == false)
            //    return

            if (!vm.model.idtipoCofinanciador) {
                utilidades.mensajeError("Verifique el tipo de fuente de financiacion.", false); return false;
            }

            var valorFondo = $('#selFondo option:selected').val();

            if (valorFondo === '') {
                utilidades.mensajeError("Debe seleccionar un fondo.", false); return false;
            }

            if (vm.fondoVisible && !vm.model.idtipoCofinanciador) {
                utilidades.mensajeError("Debe seleccionar un fondo.", false); return false;
            }

            if (vm.proyectoVisible && !vm.model.BpinBusqueda) {
                utilidades.mensajeError("Debe seleccionar un Bpin.", false); return false;
            }

            if (vm.rubroVisible && !vm.model.idRubro) {
                utilidades.mensajeError("Debe seleccionar un Rubro.", false); return false;
            }

            if (vm.privadoVisible && vm.model.NombrePrivado == "") {
                utilidades.mensajeError("Debe ingresar el nombre del cofinanciador privado.", false); return false;
            }

            var cofinanciador = "";
            var fuenteCofinanciadorId = "";
            var codigoCofinanciador = "";

            if (vm.fondoVisible) {
                codigoCofinanciador = vm.model.idtipoCofinanciador;
                fuenteCofinanciadorId = vm.model.idFondo;

            }
            if (vm.proyectoVisible) {
                codigoCofinanciador = vm.model.idtipoCofinanciador;
                fuenteCofinanciadorId = vm.proyectoIdBpin;
                codigoCofinanciador = vm.model.BpinBusqueda;//vm.model.Nombre;
            }

            if (vm.rubroVisible) {
                codigoCofinanciador = vm.model.idtipoCofinanciador;
                fuenteCofinanciadorId = 1;//vm.idRubroSeleccionado;
                codigoCofinanciador = vm.model.idRubro;//vm.model.Nombre;
            }

            if (vm.privadoVisible) {
                codigoCofinanciador = vm.model.idtipoCofinanciador;
                fuenteCofinanciadorId = 1;
                cofinanciador = vm.model.NombrePrivado;
                codigoCofinanciador = vm.model.NombrePrivado;
            }

            var params = {
                proyectoId: vm.proyectoId,
                fuenteId: vm.idFuente,
                tipoCofinanciadorId: vm.model.idtipoCofinanciador,

                cofinanciador: cofinanciador,      //vm.model.Nombre,
                fuenteCofinanciadorId: fuenteCofinanciadorId, //codigoCofinanciador,
                codigoCofinanciador: codigoCofinanciador,       //vm.model.Nombre,
            };

            recursosAjustesServicio.agregarDatosAdicionales(params, usuarioDNP, $uibModalInstance.idInstancia, $uibModalInstance.idAccion, $uibModalInstance.idNivel)
                .then(function (response) {
                    let exito = response.data;
                    if (exito.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        $sessionStorage.close();
                    }
                    else {
                        var mensaje = response.data.Mensaje;
                        utilidades.mensajeError(mensaje.substr(mensaje.indexOf(':') + 1), false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });

        }

        function eliminarDatoAdicional(cofinanciadorId) {

            //var idInstancia = $sessionStorage.idNivel;
            var idInstancia = $uibModalInstance.idNivel;
            return recursosAjustesServicio.eliminarDatosAdicionales(cofinanciadorId, usuarioDNP, idInstancia)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        $sessionStorage.close();
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    }
                    init();

                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }

    }

    angular.module('backbone').controller('ajustesModalAgregarDatosAdicionalesController', ajustesModalAgregarDatosAdicionalesController);

})();
