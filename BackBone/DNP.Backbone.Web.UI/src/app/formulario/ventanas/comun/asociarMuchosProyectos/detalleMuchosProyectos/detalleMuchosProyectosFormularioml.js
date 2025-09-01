(function () {
    'use strict';

    detalleMuchosProyectosFormularioml.$inject = [
        '$sessionStorage',
        'sesionServicios',
        'utilidades',
        'flujoServicios',
        '$scope',
        'trasladosServicio',
        'justificacionCambiosServicio',
        'constantesBackbone',
        'modalActualizaEstadoAjusteProyectoServicio',
        '$location',
        'comunesServicio'
    ];

    function detalleMuchosProyectosFormularioml(
        $sessionStorage,
        sesionServicios,
        utilidades,
        flujoServicios,
        $scope,
        trasladosServicio,
        justificacionCambiosServicio,
        constantesBackbone,
        modalActualizaEstadoAjusteProyectoServicio,
        $location,
        comunesServicio
    ) {

        var vm = this;
        vm.lang = "es";
        //vm.estadoAlertaTemplate = 'src/app/panelPrincial/modales/tramite/plantillaSeleccionar.html';
        $sessionStorage.existecontradistribucion = false;
        vm.IdEntidad = $sessionStorage.idEntidad;
        vm.eliminarAsociacion = eliminarAsociacion;
        vm.actualizaEstado = actualizaEstado;
        vm.aceptar = aceptar;
        vm.cancelar = cancelar;
        vm.devolverSeleccion = devolverSeleccion;
        vm.BPIN = '';
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;
        vm.tituloMensaje = "";
        vm.mensaje = "";
        vm.textoBuscar = '';
        vm.checkSeleccionar = false;
        vm.estadoTramite = '';
        vm.estadoAjusteCreado = false;
        vm.proyectoAsociado = false;
        vm.proyectoId = 0;
        vm.datosproyecto = {};
        vm.listavalores = [];
        $sessionStorage.EstadoAsociacionVF = '';
        $sessionStorage.EstadoAjusteCreado = false;
        $sessionStorage.EstadoDNpAplicado = true;
        vm.nombreEstadoAsociacion = 'Proyectos asociados';
        vm.nombreLeyendaML = 'Los proyectos que requieran ajustar sus valores que previamente fueron asignados a políticas transversales deberán enviar una solicitud de ajuste al' +
            ' correo politicatransversal@dnp.gov.co. Esta condición pausará el formulario, hasta que se apruebe la solicitud del cambio requerido sobre la política';
        vm.estadoAsociacion = true;
        vm.cantProyectosAsociados = 0;
        vm.visibleValidar = true;
        vm.Editar = "EDITAR";
        vm.disabled = true;
        vm.datosproyecto = {};
        vm.BPINesParaAjustarValores = [];
        vm.listaValoresAComparar = [];
        vm.envioSolicitudPendiente = $sessionStorage.envioSolicitudPendiente === undefined ? false : $sessionStorage.envioSolicitudPendiente;
        /*vm.habilitaBotones = $sessionStorage.nombreAccion.includes($sessionStorage.listadoAccionesTramite[0].Nombre) && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1*/
        vm.habilitaBotones = true;

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        //Para mostar los botones en el paso 3


        //Valida que este en el paso 3 para mostrar los botones
        vm.mostrarPaso3 = $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelElaboracionConcepto
        vm.mostrarPaso1 = true;
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;


        vm.tituloMensaje = "";
        vm.mensaje = ""
        $scope.$watch('vm.rolanalista', function () {
            if (vm.rolanalista !== '' && vm.rolanalista !== undefined && vm.rolanalista !== null) {
                vm.habilitaBotonesPaso3 = vm.rolanalista.toLowerCase() === 'true' && $sessionStorage.nombreAccion.includes('Elaboración concepto') && !vm.envioSolicitudPendiente && !$sessionStorage.soloLectura ? true : false;
            }
        });
        //Verifica que el concepto esta pendiente deshabilita todos lo botones del paso 3
        $scope.$watch('vm.deshabilitar', function () {
            if (vm.deshabilitar === "true") {
                vm.habilitaBotonesPaso3 = false && vm.rolanalista.toLowerCase() === 'true' && $sessionStorage.nombreAccion.includes('Elaboración concepto') && !vm.envioSolicitudPendiente && !$sessionStorage.soloLectura ? true : false;
            }
            else if (vm.deshabilitar === "false") {
                vm.habilitaBotonesPaso3 = true && vm.rolanalista.toLowerCase() === 'true' && $sessionStorage.nombreAccion.includes('Elaboración concepto') && !vm.envioSolicitudPendiente && !$sessionStorage.soloLectura ? true : false;
            }

        });

        vm.peticionObtenerInbox = {
            // ReSharper disable once UndeclaredGlobalVariableUsing
            IdUsuario: usuarioDNP,
            IdObjeto: idTipoTramite,
            // ReSharper disable once UndeclaredGlobalVariableUsing
            Aplicacion: nombreAplicacionBackbone,
            IdInstancia: $sessionStorage.idInstanciaIframe,
            ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            IdsEtapas: getIdEtapa()
        };

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            }
        });

        vm.init = function () {

            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                        if (vm.tramiteid != undefined && vm.tramiteid != '') {
                            EstadoBotones();
                            ObtenerProyectosTramite();
                            vm.disabled = true;
                        }
                    }
                }, true);

            $scope.$watch(() => vm.actualizadetalle
                , (newVal, oldVal) => {
                    if (newVal) {
                        if (vm.tramiteid != undefined && vm.tramiteid != '')
                            ObtenerProyectosTramite();
                    }
                }, true);



        }

        function EstadoBotones() {

            if (vm.tipotramiteid === '52') {
                var estadoPaso = $sessionStorage.listadoAccionesTramite.find(x => x.Nombre === $sessionStorage.nombreAccion);
                var rolapresupuestopreliminar = $sessionStorage.usuario.roles.find(x => x.IdRol === constantesBackbone.idRPresupuesto.toLowerCase());
                var estadotramite = $sessionStorage.InstanciaSeleccionada.estadoTramite == "Cancelado"|| estadoPaso.Estado == "Ejecutada" || (estadoPaso.Estado !== "Ejecutada" && rolapresupuestopreliminar === undefined) ? true : false;
                if (estadotramite == true) {
                    vm.habilitaBotones = false;
                    vm.mostrarPaso1 = false;
                }
                else {
                    var rolactual = $sessionStorage.usuario.roles.find(x => x.IdRol === constantesBackbone.idRPresupuesto.toLowerCase());
                    var permisosEntidadUsuario = $sessionStorage.usuario.permisos.Entidades.find(ent => ent.NombreEntidad.includes($sessionStorage.InstanciaSeleccionada.entidad));
                    if (permisosEntidadUsuario != undefined) {
                        var rolUsuarioEntidad = permisosEntidadUsuario.Roles.find(r => r.Nombre.includes('Presupuesto - preliminar'));
                        if (rolUsuarioEntidad != undefined) {
                            vm.habilitaBotones = $sessionStorage.listadoAccionesTramite[0].EstadoAccionPorInstanciaId == 0;
                            vm.mostrarPaso1 = $sessionStorage.listadoAccionesTramite[0].EstadoAccionPorInstanciaId == 0;
                        }
                    }

                }

            }
            if (vm.tipotramiteid === '49') {
                var estadoPaso = $sessionStorage.listadoAccionesTramite.find(x => x.Nombre === $sessionStorage.nombreAccion);
                var estadotramite = estadoPaso.Estado == "Cancelado" || estadoPaso.Estado === "Ejecutada";
                if (estadotramite == true) {
                    vm.habilitaBotones = false;
                    vm.mostrarPaso1 = false;
                }
                else {
                    var rolUsuarioEntidad = $sessionStorage.usuario.roles.find(x => x.Nombre.includes('Analista'));
                    if (rolUsuarioEntidad != undefined) {
                        vm.habilitaBotones = $sessionStorage.listadoAccionesTramite[0].EstadoAccionPorInstanciaId == 0;
                        vm.mostrarPaso1 = $sessionStorage.listadoAccionesTramite[0].EstadoAccionPorInstanciaId == 0;
                    }
                }
            }
        }


        function ObtenerProyectosTramite() {

            vm.listaEntidadesProy = [];
            vm.listaGrupoEntidades = [];
            vm.listaGrupoProyectos = [];
            vm.datoproyectosTramite = [];
            vm.listaproyectosEntidad = [];
            vm.listaEntidadesGrilla = [];
            vm.listaGrillaProyectos = [];
            //vm.gridOptions.data = vm.listaGrillaProyectos;
            // vm.gridOptions.columnDefs = [];

            vm.ValorTotalMontoRC = 0;
            vm.ValorTotalMontoPropiosRC = 0;
            vm.ValorTotalMontoNacionRC = 0;
            vm.ValorTotalMontoRCC = 0;
            vm.ValorTotalMontoNacionRCC = 0;
            vm.ValorTotalMontoPropiosRCC = 0;

            comunesServicio.obtenerProyectosTramite(vm.tramiteid)
                .then(function (response) {
                    if (response.data !== null && response.data.length > 0) {
                        vm.listaEntidadesProy = response.data;
                        vm.cantProyectosAsociados = vm.listaEntidadesProy.length;
                        var listaproy = [];
                        vm.listaEntidadesProy.forEach(entidad => {
                            if (!vm.listaGrupoEntidades.find(ent => ent.EntidadId === entidad.EntidadId)) {
                                const { Sector, NombreEntidad, EntidadId } = entidad;
                                vm.listaGrupoEntidades.push({ Sector, NombreEntidad, EntidadId });
                            }
                            listaproy.push(entidad.BPIN);
                        });

                        vm.listaGrupoEntidades.forEach(entidad => {

                            vm.listaGrupoProyectos = [];

                            vm.listaEntidadesProy.forEach(proyectoentidad => {
                                if (proyectoentidad.EntidadId === entidad.EntidadId) {
                                    proyectoentidad.ValorMontoProyectoNacion = formatearNumero(proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion);
                                    proyectoentidad.ValorMontoProyectoNacionSSF = formatearNumero(proyectoentidad.ValorMontoProyectoNacionSSF === null ? '0' : proyectoentidad.ValorMontoProyectoNacionSSF);
                                    proyectoentidad.ValorMontoProyectoPropios = formatearNumero(proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios);
                                    proyectoentidad.ValorMontoTramiteNacion = formatearNumero(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion);
                                    proyectoentidad.ValorMontoTramitePropios = formatearNumero(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios);
                                    proyectoentidad.ValorMontoTramiteNacionSSF = formatearNumero(proyectoentidad.ValorMontoTramiteNacionSSF === null ? '0' : proyectoentidad.ValorMontoTramiteNacionSSF);
                                
                                    if (!vm.listaGrupoProyectos.find(p => p.EntidadId === proyectoentidad.EntidadId)) {
                                        const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoNacionSSF, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                            ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal } = proyectoentidad;
                                        vm.listaGrupoProyectos.push({
                                            BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoNacionSSF, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                            ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal
                                        });
                                    }
                                }
                            });
                        });

                        /* Ajuste mientras lucho revisa. Marco Montoya. Bug. 37088. 17/11/2022*/
                        vm.ValorTotalMontoRC = 0;
                        vm.ValorTotalMontoNacionRC = 0;
                        vm.ValorTotalMontoPropiosRC = 0;
                        vm.ValorTotalMontoRCC = 0;
                        vm.ValorTotalMontoNacionRCC = 0;
                        vm.ValorTotalMontoPropiosRCC = 0;
                        vm.listaEntidadesProy.forEach(proyectoentidad => {
                            var MontoTramiteNacion = desFormatearNumero(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion);
                            MontoTramiteNacion = parseFloat(MontoTramiteNacion.toString().includes(",") ? MontoTramiteNacion.replaceAll(",", ".") : MontoTramiteNacion);
                            var MontoTramiteNacionSSF = desFormatearNumero(proyectoentidad.ValorMontoTramiteNacionSSF === null ? '0' : proyectoentidad.ValorMontoTramiteNacionSSF);
                            MontoTramiteNacionSSF = parseFloat(MontoTramiteNacionSSF.toString().includes(",") ? MontoTramiteNacionSSF.replaceAll(",", ".") : MontoTramiteNacionSSF);
                            var MontoTramitePropios = desFormatearNumero(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios);
                            MontoTramitePropios = parseFloat(MontoTramitePropios.toString().includes(",") ? MontoTramitePropios.replaceAll(",", ".") : MontoTramitePropios);

                            if (proyectoentidad.TipoProyecto === 'Credito') {
                                vm.ValorTotalMontoRC += (MontoTramiteNacion + MontoTramiteNacionSSF + MontoTramitePropios);
                                vm.ValorTotalMontoNacionRC += (MontoTramiteNacion + MontoTramiteNacionSSF);
                                vm.ValorTotalMontoPropiosRC += (MontoTramitePropios);
                            }
                            else {
                                vm.ValorTotalMontoRCC += (MontoTramiteNacion + MontoTramiteNacionSSF + MontoTramitePropios);
                                vm.ValorTotalMontoNacionRCC += (MontoTramiteNacion + MontoTramiteNacionSSF);
                                vm.ValorTotalMontoPropiosRCC += (MontoTramitePropios);
                            }
                        });
                        /* Fin ajuste.*/

                        vm.ValorTotalMontoRC = formatearNumero(vm.ValorTotalMontoRC);
                        vm.ValorTotalMontoNacionRC = formatearNumero(vm.ValorTotalMontoNacionRC);
                        vm.ValorTotalMontoPropiosRC = formatearNumero(vm.ValorTotalMontoPropiosRC);
                        vm.ValorTotalMontoRCC = formatearNumero(vm.ValorTotalMontoRCC);
                        vm.ValorTotalMontoNacionRCC = formatearNumero(vm.ValorTotalMontoNacionRCC);
                        vm.ValorTotalMontoPropiosRCC = formatearNumero(vm.ValorTotalMontoPropiosRCC);

                        vm.listaGrupoProyectos = [];
                        vm.listaEntidadesProy.forEach(proyectoentidad => {
                            const { BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoNacionSSF, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal } = proyectoentidad;
                            vm.listaGrupoProyectos.push({
                                BPIN, NombreProyecto, ProyectoId, TipoProyecto, Estado, EntidadId, ValorMontoProyectoNacion, ValorMontoProyectoNacionSSF, ValorMontoProyectoPropios, ValorMontoTramiteNacion,
                                ValorMontoTramitePropios, Programa, SubPrograma, EstadoActualizacion, CodigoPresupuestal
                            });

                            vm.gestionarListaProgramasSubs_asociados(proyectoentidad);
                        });

                        vm.listaGrillaProyectos = [];
                        vm.listaEntidadesProy.forEach(entidad => {
                            vm.listaGrillaProyectos.push({
                                entidad: entidad.NombreEntidad,
                                BPIN: entidad.BPIN,
                                NombreProyecto: entidad.NombreProyecto,
                                ProyectoId: entidad.ProyectoId,
                                TipoProyecto: entidad.TipoProyecto,
                                EntidadId: entidad.EntidadId,
                                Estado: entidad.Estado,
                                ValorMontoProyectoNacion: entidad.ValorMontoProyectoNacion === null ? '0' : entidad.ValorMontoProyectoNacion,
                                ValorMontoProyectoNacionSSF: entidad.ValorMontoProyectoNacionSSF === null ? '0' : entidad.ValorMontoProyectoNacionSSF,
                                ValorMontoProyectoPropios: entidad.ValorMontoProyectoPropios === null ? '0' : entidad.ValorMontoProyectoPropios,
                                ValorMontoTramiteNacion: entidad.ValorMontoTramiteNacion === null ? '0' : entidad.ValorMontoTramiteNacion,
                                ValorMontoTramiteNacionSSF: entidad.ValorMontoTramiteNacionSSF === null ? '0' : entidad.ValorMontoTramiteNacionSSF,
                                ValorMontoTramitePropios: entidad.ValorMontoTramitePropios === null ? '0' : entidad.ValorMontoTramitePropios,
                                Programa: entidad.Programa,
                                SubPrograma: entidad.SubPrograma,
                                EstadoActualizacion: entidad.EstadoActualizacion,
                                CodigoPresupuestal: entidad.CodigoPresupuestal,

                            });

                        });

                        comunesServicio.obtenerInstanciasActivasProyectos(listaproy)
                            .then(function (resul) {
                                vm.listaEntidadesProy.forEach(entproy => {
                                    if (resul.data.find(res => res === entproy.BPIN))
                                        entproy.TieneInstancia = true;
                                    else
                                        entproy.TieneInstancia = false;
                                });
                                //if (resul.data.length === 0)
                                //    entidadx.TieneInstancia = false;
                                ///* retorno = false;*/
                                //else
                                //    entidadx.TieneInstancia = true;
                            }
                            );


                    }
                    else
                        eliminarCapitulosModificados();
                }).then(function (resultadofinal) {
                    vm.actualizacomponentes = vm.actualizacomponentes + '1';
                    vm.listaValoresAComparar = angular.copy(vm.listaEntidadesProy);
                });;
        }

        vm.gestionarListaProgramasSubs_asociados = function (proyectoentidad) {
            if (vm.tipotramiteid == 1) {
                if (!$sessionStorage.listadosubprogramas.find(ent => ent.id === proyectoentidad.SubPrograma)) {
                    var subprograma = { id: proyectoentidad.SubPrograma };
                    $sessionStorage.listadosubprogramas.push(subprograma);
                }
            }
            if (vm.tipotramiteid == 8 && proyectoentidad.TipoProyecto === 'Contracredito')
                $sessionStorage.existecontradistribucion = true;
            if (vm.tipotramiteid == 2) {
                if (!$sessionStorage.listadosubprogramas.find(ent => ent.id === proyectoentidad.SubPrograma)) {
                    var subprograma = { id: proyectoentidad.SubPrograma };
                    $sessionStorage.listadosubprogramas.push(subprograma);
                }
                if (!$sessionStorage.listadoprogramas.find(ent => ent.id === proyectoentidad.Programa)) {
                    var programa = { id: proyectoentidad.Programa };
                    $sessionStorage.listadoprogramas.push(programa);
                }
            }
        }


        function eliminarAsociacion(proyectoId, bpin, tipoProyecto) {
            if (proyectoId === undefined || proyectoId === '' || proyectoId === 0) {
                utilidades.mensajeError('No hay proyectos asociados.');
            }
            else {
                utilidades.mensajeWarning("Con esto, la información del proyecto definida en otros espacios del trámite se perderá. ¿Esta seguro de continuar?",
                    function funcionContinuar() {
                        var eliminarAsociacionDto = {
                            TramiteId: $sessionStorage.tramiteId,
                            InstanciaId: $sessionStorage.idInstancia,
                            ProyectoId: proyectoId,
                            IdUsuario: $sessionStorage.usuario.permisos.IdUsuarioDNP,
                        };

                        //tramiteVigenciaFuturaServicio.eliminarAsociacion(eliminarAsociacionDto)
                        trasladosServicio.eliminarProyectoTramite(eliminarAsociacionDto, eliminarAsociacionDto)
                            .then(function (response) {
                                if (response.status == "200") {
                                    vm.actualizadetalle++;
                                    utilidades.mensajeSuccess("Se elimino la información del proyecto con BPIN " + bpin
                                        + " desde este proceso de trámite", false, false, false, "El proyecto ha sido desasociado con éxito.");
                                    if (vm.tipotramiteid == 8 && tipoProyecto === 'Contracredito')
                                        $sessionStorage.existecontradistribucion = false;
                                    $sessionStorage.listadosubprogramas = [];
                                    $sessionStorage.listadoprogramas = [];
                                    var proyAdelete = vm.listaEntidadesProy.find(x => x.ProyectoId === proyectoId);
                                    vm.listaEntidadesProy.splice(proyAdelete, 1);
                                    vm.listaEntidadesProy.forEach(proyectoentidad => {
                                        vm.gestionarListaProgramasSubs_asociados(proyectoentidad);
                                    });
                                    vm.modificodatos = '1';
                                    if (vm.tipotramiteid === 8) {
                                        trasladosServicio.eliminarInstanciaCerrada_AbiertaProyectoTramite($sessionStorage.idInstanciaIframe, proyectoId)
                                        then(function (result) {
                                            if (result.status === "200") {
                                                var texto = "El proyecto con BPIN " + bpin + " ha sido desasociado del tramite " + $sessionStorage.idObjetoNegocio;
                                                notificarUsuariosPorInstanciaPadre($sessionStorage.idInstanciaIframe, "Desasociar proyecto", texto);
                                            }
                                        });
                                    }

                                    //Es posible que para el proceso de creación de Ajuste y validaciones se requiera el siguiente código. la guia de como implementarlo la encuentran en el archivo
                                    // DNP.Backbone.Web.UI\src\app\formulario\ventanas\tramiteVigenciaFutura\componentes\asociarProyectovf\seleccionProyecto\seleccionProyectosvfController.js

                                    ////para guardar los capitulos modificados y que se llenen las lunas, este modificado en 0
                                    //ObtenerSeccionCapitulo();
                                    //eliminarCapitulosModificados(vm.seccionCapituloAsociarid);
                                    //eliminarCapitulosModificados(vm.seccionCapituloCrear);
                                    //$sessionStorage.EstadoAjusteCreado = false;
                                    //vm.estadoAjusteCreado = $sessionStorage.EstadoAjusteCreado;
                                    //$sessionStorage.EstadoAjusteFinalizado = false;
                                    //vm.estadoAjusteFinalizado = $sessionStorage.EstadoAjusteFinalizado;
                                    /////Ajuste Estabilizacion
                                    //// $sessionStorage.proyectoId = 0;
                                    //$sessionStorage.BPIN = '';
                                    //$sessionStorage.nombreProyecto = '';
                                    //vm.BPIN = $sessionStorage.BPIN;
                                    //vm.proyecto = $sessionStorage.nombreProyecto;
                                }
                                else {
                                    //utilidades.mensajeError("Error al realizar la operación", false);
                                }
                            })
                            .catch(error => {
                                if (error.status == "400") {
                                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                    return;
                                }
                                utilidades.mensajeError("Error al realizar la operación");
                            });
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "El proyecto será desasociado."
                )
            }
        }

        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }



        function actualizaEstado(bpin) {
            vm.ajuste = 0;
            vm.BPIN = bpin;
            if (vm.model !== undefined && vm.model.observacion !== undefined) {
                vm.model.observacion = '';
            }

            utilidades.mensajeWarning(
                "La ampliación del concepto se reflejá en la pestaña Justificación de este formulario, cuando se responda a esta solicitud desde el formulario Ajuste del proyecto.<br/>¿Está seguro de continuar? ",
                function funcionContinuar() {
                    vm.tituloMensaje = "Solicitud para ampliar concepto";
                    vm.mensaje = "Escriba sus razones de devolver el formularío para ampliar concepto* ";
                    angular.element('#IPModal').modal('show');
                },
                function funcionCancelar(reason) {
                    console.log("reason", reason);
                },
                "Aceptar",
                "Cancelar",
                "El proyecto se devolverá a Ajuste del proyecto para ampliar concepto");
        }


        function aceptar() {

            if (vm.model !== undefined && vm.model.observacion !== undefined) {

                //Cierra la pantalla modal
                angular.element('#IPModal').modal('hide');

                if (vm.BPINesParaAjustarValores.length != 0) {
                    vm.BPINesParaAjustarValores.forEach(x => {
                        modalActualizaEstadoAjusteProyectoServicio.actualizaEstadoAjusteProyecto(1, x, $sessionStorage.tramiteId, vm.model.observacion)
                            .then(function (response) {
                                let exito = response.data.Exito;

                                if (exito) {
                                    let mensajeTitulo = "", mensaje = "";

                                    vm.conceptoHabilitado = true;
                                    vm.valorHabilitado = true;
                                    mensajeTitulo = "El formulario se ha devuelto al paso 1 Crear trámite. <br/>  El proyecto se ha devuelto a Control  posterior";
                                    mensaje = "Usted puede acceder a este proceso desde la consola  de procesos."

                                    utilidades.mensajeSuccess(mensaje, false, false, false, mensajeTitulo);
                                    $location.path("/tramites/ej");
                                    vm.BPIN = '';
                                }
                                else {
                                    utilidades.mensajeError("Error al realizar la operación", false);
                                }
                            })
                            .catch(error => {
                                if (error.status == 400) {
                                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                    return;
                                }
                                utilidades.mensajeError("Error al realizar la operación");
                            });
                    });

                    vm.BPINesParaAjustarValores = [];
                }
                else {

                    modalActualizaEstadoAjusteProyectoServicio.actualizaEstadoAjusteProyecto(vm.ajuste, vm.BPIN, $sessionStorage.tramiteId, vm.model.observacion)
                        .then(function (response) {
                            let exito = response.data.Exito;

                            if (exito) {
                                let mensajeTitulo = "", mensaje = "";

                                vm.conceptoHabilitado = true;
                                mensajeTitulo = "El proyecto fue devuelto al formulario de Ajustes del proyecto";

                                utilidades.mensajeSuccess(mensaje, false, false, false, mensajeTitulo);

                                ObtenerProyectosTramite();
                                $sessionStorage.EstadoDNpAplicado = false;
                                vm.callback();

                                vm.BPIN = '';
                            }
                            else {
                                utilidades.mensajeError("Error al realizar la operación", false);
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
            }
            else {
                utilidades.mensajeError("Incluir la observación", false); return false;
            }
        }


        function cancelar() {
            $('#tablatest input[type="checkbox"]:checked').each(function () {
                $(this).prop('checked', false);
            });
        }

        function devolverSeleccion() {
            var selected = new Array();
            $('#tablatest input[type="checkbox"]:checked').each(function () {
                selected.push($(this).attr('id'));
            });

            var entities = vm.listaEntidadesProy.filter(entity => selected.includes(entity.ProyectoId.toString()));

            var tiposProyecto = entities.map(entidad => entidad.TipoProyecto);

            var listatipoCredito = [];
            var listatipoContracredito = [];
            var error = undefined;

            entities.map(function (item) {
                if (item.TipoProyecto === "Credito")
                    listatipoCredito.push(item);
            });

            entities.map(function (item) {
                if (item.TipoProyecto === "Contracredito")
                    listatipoContracredito.push(item);
            });

            listatipoContracredito.map(function (item) {
                var indxtp = listatipoCredito.findIndex(x => x.Programa === item.Programa && x.SubPrograma === item.SubPrograma);
                if (indxtp < 0)
                    error = "Hay proyectos contracredito sin un proyecto credito del mismo programa y subprograma.";
            });
            if (error === undefined) {
                listatipoCredito.map(function (item) {
                    var indxtp = listatipoContracredito.findIndex(x => x.Programa === item.Programa && x.SubPrograma === item.SubPrograma);
                    if (indxtp < 0)
                        error = "Hay proyectos credito sin un proyecto contracredito del mismo programa y subprograma. ";
                });
            }

            if (error !== undefined) {
                utilidades.mensajeError(error);
                return;
            }

            if (tiposProyecto.includes("Credito") && tiposProyecto.includes("Contracredito")) {

                if (vm.model !== undefined && vm.model.observacion !== undefined) {
                    vm.model.observacion = '';
                }

                vm.BPINesParaAjustarValores = entities.map(entidad => entidad.BPIN);

                utilidades.mensajeWarning(
                    "Este formulario desaparecerá  de sus procesos pendientes hasta  que reciba  nuevamente  los permisos de edición.<br/>¿Está seguro de continuar? ",
                    function funcionContinuar() {
                        vm.tituloMensaje = "Solicitud para ajustar valores";
                        vm.mensaje = "Escriba sus razones de devolver el formularío para ajustar valores*";

                        angular.element('#IPModal').modal('show');
                    },
                    function funcionCancelar(reason) {
                        console.log("reason", reason);
                    },
                    "Aceptar",
                    "Cancelar",
                    "El proyecto se devolverá al paso 1 Crear Trámite.<br/> El proyecto se devolvera a control posterior");
            }
            else {
                utilidades.mensajeError("Debe seleccionar al menos un 'Credito' y un 'Contracredito'");
            }

            console.log(selected);
        }



        function formatearNumero(value) {
            //var numerotmp = (value == '' || Number.isNaN(value)) ? 0 : value.toString().replaceAll('.', '');
            //return parseFloat(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
            return new Intl.NumberFormat('es-co', {
                minimumFractionDigits: 2,
            }).format(value);
        }

        function desFormatearNumero(value) {
            return (value == '' || value == 'NaN') ? 0 : value.replaceAll(".", "");
        }

        function getIdEtapa() {
            var idEtapa = [];
            switch (vm.etapa) {
                case 'pl':
                    idEtapa = [constantesBackbone.idEtapaPlaneacion, constantesBackbone.idEtapaViabilidadRegistro, constantesBackbone.idEtapaAjustes, constantesBackbone.idEtapaPriorizacion];
                    break;
                case 'pr':
                    idEtapa = [constantesBackbone.idEtapaProgramacion];
                    break;
                case 'gr':
                    idEtapa = [constantesBackbone.idEtapaGestionRecursos, constantesBackbone.idEtapaSolicitudRecursos, constantesBackbone.idEtapaRevisionRequisitos, constantesBackbone.idEtapaAprobacion, constantesBackbone.idEtapaAjustesGR];
                    break;
                case 'ej':
                    idEtapa = [constantesBackbone.idEtapaEjecucion, constantesBackbone.idEtapaNuevaEjecucion, constantesBackbone.idEtapaProgramacionEjecucion, constantesBackbone.idEtapaAjustesEjecucion, constantesBackbone.idEtapaTramitesEjecucion, constantesBackbone.idEtapaSeguimientoControl];
                    break;
                case 'se':
                    idEtapa = [];
                    break;
                case 'ev':
                    idEtapa = [constantesBackbone.idEtapaEvaluacion, constantesBackbone.idEtapaCortoPlazo, constantesBackbone.idEtapaMedianoPlazo, constantesBackbone.idEtapaLargoPlazo];
                    break;
            }
            return idEtapa;
        }

        vm.getEstiloBtnGuardar = function () {
            if (vm.disabled == false)
                return "btnguardarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        vm.getEstiloBtnEditar = function () {
            if (vm.cantProyectosAsociados > 0)
                return "btneditarDNP";
            else
                return "btnguardarDisabledDNP";
        }

        vm.ActivarEditar = function () {
            var panel = document.getElementById('Guardar');
            if (vm.disabled == true) {
                vm.Editar = "CANCELAR";
                vm.disabled = false;
                //panel.classList.replace("btnguardarDisabledDNP", "btnguardarDNP");
            }
            else {
                vm.Editar = "EDITAR";
                vm.disabled = true;
                //panel.classList.replace("btnguardarDNP", "btnguardarDisabledDNP");
                ObtenerProyectosTramite();
            }
        }

        vm.focus = function (event) {
            event.target.value = event.target.value.length > 0 ? desFormatearNumero(event.target.value) : '';
        }

        vm.actualizarMontos = function (event, fila, valorIndex) {
            var sumPropiosRC = 0;
            var sumPropiosRCC = 0;
            var sumNacionRC = 0;
            var sumNacionRCC = 0;
            var sumTotalRC = 0;
            var sumTotalRCC = 0;
            var valMontoPropios = 0;

            vm.listaEntidadesProy.forEach(entProyecto => {
                var MontoTramiteNacion = parseFloat((entProyecto.ValorMontoTramiteNacion === null || entProyecto.ValorMontoTramiteNacion === '') ? '0' : entProyecto.ValorMontoTramiteNacion.replace(",", "."));
                var MontoTramiteNacionSSF = parseFloat((entProyecto.ValorMontoTramiteNacionSSF === null || entProyecto.ValorMontoTramiteNacionSSF === '') ? '0' : entProyecto.ValorMontoTramiteNacionSSF.replace(",", "."));
                var MontoTramitePropios = parseFloat((entProyecto.ValorMontoTramitePropios === null || entProyecto.ValorMontoTramitePropios === '') ? '0' : entProyecto.ValorMontoTramitePropios.replace(",", "."));

                if (entProyecto.TipoProyecto === 'Credito') {
                    sumTotalRC += (MontoTramiteNacion + MontoTramiteNacionSSF + MontoTramitePropios);
                    sumNacionRC += (MontoTramiteNacion + MontoTramiteNacionSSF);
                    sumPropiosRC += (MontoTramitePropios);
                }
                else {
                    sumTotalRCC += (MontoTramiteNacion + MontoTramiteNacionSSF + MontoTramitePropios);
                    sumNacionRCC += (MontoTramiteNacion + MontoTramiteNacionSSF);
                    sumPropiosRCC += (MontoTramitePropios);
                }

            });

            vm.ValorTotalMontoRC = formatearNumero(sumTotalRC);
            vm.ValorTotalMontoNacionRC = formatearNumero(sumNacionRC);
            vm.ValorTotalMontoPropiosRC = formatearNumero(sumPropiosRC);
            vm.ValorTotalMontoRCC = formatearNumero(sumTotalRCC);
            vm.ValorTotalMontoNacionRCC = formatearNumero(sumNacionRCC);
            vm.ValorTotalMontoPropiosRCC = formatearNumero(sumPropiosRCC);

            if (!isNaN(parseFloat(event.target.value.replace(",", "."))))
                event.target.value = formatearNumero(parseFloat(event.target.value.replace(",", ".")));
            else
                event.target.value = '';
        }

        vm.actualizarValores = function (response) {
            completarActualizarValores();

            vm.Editar = "EDITAR";
            vm.disabled = true;
        }



        function completarActualizarValores() {
            //var validarValores = 0;
            //var montoCompararNacion;
            //var montoCompararPropios;
            //var conteoContracredito = 0;

            comunesServicio.obtenerDatosProyectosPorTramite(vm.tramiteid).then(
                function (respuesta) {
                    vm.datosproyecto = respuesta.data;
                }).then(function () {

                    vm.listaEntidadesProy.forEach(proyectoentidad => {
                        proyectoentidad.ValorMontoProyectoNacion = proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion.toString().includes(".") ? proyectoentidad.ValorMontoProyectoNacion.toString().replaceAll(".", "") : proyectoentidad.ValorMontoProyectoNacion;
                        proyectoentidad.ValorMontoProyectoNacionSSF = proyectoentidad.ValorMontoProyectoNacionSSF === null ? '0' : proyectoentidad.ValorMontoProyectoNacionSSF.toString().includes(".") ? proyectoentidad.ValorMontoProyectoNacionSSF.toString().replaceAll(".", "") : proyectoentidad.ValorMontoProyectoNacionSSF;
                        proyectoentidad.ValorMontoProyectoPropios = proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios.toString().includes(".") ? proyectoentidad.ValorMontoProyectoPropios.toString().replaceAll(".", "") : proyectoentidad.ValorMontoProyectoPropios;
                        proyectoentidad.ValorMontoTramiteNacion = proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion.toString().includes(".") ? proyectoentidad.ValorMontoTramiteNacion.toString().replaceAll(".", "") : proyectoentidad.ValorMontoTramiteNacion;
                        proyectoentidad.ValorMontoTramiteNacionSSF = proyectoentidad.ValorMontoTramiteNacionSSF === null ? '0' : proyectoentidad.ValorMontoTramiteNacionSSF.toString().includes(".") ? proyectoentidad.ValorMontoTramiteNacionSSF.toString().replaceAll(".", "") : proyectoentidad.ValorMontoTramiteNacionSSF;
                        proyectoentidad.ValorMontoTramitePropios = proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios.toString().includes(".") ? proyectoentidad.ValorMontoTramitePropios.toString().replaceAll(".", "") : proyectoentidad.ValorMontoTramitePropios;

                        proyectoentidad.ValorMontoProyectoNacion = parseFloat(proyectoentidad.ValorMontoProyectoNacion === null ? '0' : proyectoentidad.ValorMontoProyectoNacion.toString().includes(",") ? proyectoentidad.ValorMontoProyectoNacion.replaceAll(",", ".") : proyectoentidad.ValorMontoProyectoNacion);
                        proyectoentidad.ValorMontoProyectoNacionSSF = parseFloat(proyectoentidad.ValorMontoProyectoNacionSSF === null ? '0' : proyectoentidad.ValorMontoProyectoNacionSSF.toString().includes(",") ? proyectoentidad.ValorMontoProyectoNacionSSF.replaceAll(",", ".") : proyectoentidad.ValorMontoProyectoNacionSSF);
                        proyectoentidad.ValorMontoProyectoPropios = parseFloat(proyectoentidad.ValorMontoProyectoPropios === null ? '0' : proyectoentidad.ValorMontoProyectoPropios.toString().includes(",") ? proyectoentidad.ValorMontoProyectoPropios.replaceAll(",", ".") : proyectoentidad.ValorMontoProyectoPropios);
                        proyectoentidad.ValorMontoTramiteNacion = parseFloat(proyectoentidad.ValorMontoTramiteNacion === null ? '0' : proyectoentidad.ValorMontoTramiteNacion.toString().includes(",") ? proyectoentidad.ValorMontoTramiteNacion.replaceAll(",", ".") : proyectoentidad.ValorMontoTramiteNacion);
                        proyectoentidad.ValorMontoTramiteNacionSSF = parseFloat(proyectoentidad.ValorMontoTramiteNacionSSF === null ? '0' : proyectoentidad.ValorMontoTramiteNacionSSF.toString().includes(",") ? proyectoentidad.ValorMontoTramiteNacionSSF.replaceAll(",", ".") : proyectoentidad.ValorMontoTramiteNacionSSF);
                        proyectoentidad.ValorMontoTramitePropios = parseFloat(proyectoentidad.ValorMontoTramitePropios === null ? '0' : proyectoentidad.ValorMontoTramitePropios.toString().includes(",") ? proyectoentidad.ValorMontoTramitePropios.replaceAll(",", ".") : proyectoentidad.ValorMontoTramitePropios);



                        //if (proyectoentidad.TipoProyecto === 'Contracredito') {
                        //    conteoContracredito++;
                        //    if (vm.datosproyecto.find(dp => dp.ProyectoId === proyectoentidad.ProyectoId)) {

                        //        var vigenciasFuturas = vm.datosproyecto.find(vf => vf.ProyectoId === proyectoentidad.ProyectoId)

                        //        montoCompararNacion = vigenciasFuturas.ValorDisponibleNacion;
                        //        montoCompararPropios = vigenciasFuturas.ValorDisponiblePropios;

                        //    }
                        //    else {
                        //        montoCompararNacion = proyectoentidad.ValorMontoProyectoNacion;
                        //        montoCompararPropios = proyectoentidad.ValorMontoProyectoPropios;
                        //    }

                        //    //if (parseFloat(proyectoentidad.ValorMontoTramiteNacion, 10) > parseFloat(montoCompararNacion, 0) || parseFloat(proyectoentidad.ValorMontoTramitePropios, 10) > parseFloat(montoCompararPropios, 0)) {
                        //    //    validarValores = 0;
                        //    //}
                        //    //else validarValores++;
                        //}
                    })
                }).then(function () {

                    //if (conteoContracredito === validarValores) {
                        comunesServicio.guardarMontosTramite(vm.listaEntidadesProy).then(function (response) {
                            if (response.data && (response.statusText === "OK" || response.status === 200)) {
                                utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                                ObtenerProyectosTramite();
                            } else {
                                swal('', "NO SE PUEDEN DESFINANCIAR LAS VIGENCIAS FUTURAS O LOS RECURSOS FOCALIZADOS EN LAS POLITCAS TRASNVERSALES DEL PROYECTO", 'error');
                            }
                        })
                    //}
                    //else {
                    //    utilidades.mensajeError("Los montos de los trámites no pueden ser mayores que los valores disponibles para proyectos con tipo de movimiento contracédito!");
                    //}
                });
        }

        function eliminarCapitulosModificados() {
            ObtenerSeccionCapitulo();
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,

            }
            justificacionCambiosServicio.eliminarCapitulosModificados(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }
        //para guardar los capitulos modificados y que se llenen las lunas
        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;


        }

    }










    angular.module('backbone').component('detalleMuchosProyectosFormularioml', {
        templateUrl: "src/app/formulario/ventanas/comun/asociarMuchosProyectos/detalleMuchosProyectos/detalleMuchosProyectosFormularioml.html",
        controller: detalleMuchosProyectosFormularioml,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            deshabilitarBotonDevolverSeccionProyecto: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            actualizadetalle: '@',
            idflujo: '@',
            actualizacomponentes: '=',
            nombrecomponentepaso: '@',
            notificacionvalidacion: '&',
            deshabilitar: '@',
            rolanalista: '@',
            modificodatos: '='
        }
    });

})();