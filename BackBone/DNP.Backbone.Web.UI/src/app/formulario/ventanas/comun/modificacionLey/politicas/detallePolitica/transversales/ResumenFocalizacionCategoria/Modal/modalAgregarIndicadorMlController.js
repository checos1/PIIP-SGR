(function () {
	'use strict';

	modalAgregarIndicadorMlController.$inject = [
		'$sessionStorage',
		'$uibModalInstance',
		'utilidades',
		'$filter',
		'modificacionLeyServicio'
	];

	function modalAgregarIndicadorMlController(
		$uibModalInstance,
		$sessionStorage,
		utilidades,
		$filter,		
        modificacionLeyServicio
	) {
		var vm = this;
		vm.init = init;
		vm.ConsultarPoliticasCategoriasIndicadores = ConsultarPoliticasCategoriasIndicadores;
		vm.cerrar = $sessionStorage.close;
		vm.actualizar;
		vm.habilitaFila;
		vm.Usuario = usuarioDNP;
		vm.idInstancia = $uibModalInstance.idInstancia;
		vm.listaPoliticasCategoriasIndicadores = null;
        vm.ProyectoSelecId = $uibModalInstance.ProyectoSelecId;
		vm.idIndicador = $uibModalInstance.idIndicador;
		vm.ListaIndicadoresSelc = [];
		vm.guardar;
        vm.CategoriaSelec = $uibModalInstance.CategoriaSelec;
        vm.PoliticaSelecId = $uibModalInstance.PoliticaSelecId;
		vm.focalizacionIdSelec;
		vm.FocalizacionId = $uibModalInstance.FocalizacionId;
		vm.indicadorSeleccionado;
		vm.categoriaId;
		vm.ListaIndicadoresAsociadosCategoriaList = [];
		vm.inhabilitar;
		vm.arregloGeneral = [];
        vm.Mensajeindicadores = "";
        vm.buscarIndicador = buscarIndicador;
        vm.LimpiarChecks = LimpiarChecks;

		function init() {
			vm.model = {
				modulos: {
					administracion: false,
					backbone: true
				}
			}
			ConsultarPoliticasCategoriasIndicadores();
		}

		function ConsultarPoliticasCategoriasIndicadores() {
            return modificacionLeyServicio.ConsultarCatalogoIndicadoresPolitica(vm.PoliticaSelecId,"OK").then(
				function (respuesta) {
					var arreglo = jQuery.parseJSON(respuesta.data);
					vm.arregloGeneral = jQuery.parseJSON(arreglo);
					var arregloIndicadores = vm.arregloGeneral.Indicadores;					
					if (respuesta.data != null && respuesta.data != "") {
						arregloIndicadores.forEach(itemIndicador => {							
							vm.ListaIndicadoresAsociadosCategoriaList.push(
									{
                                        Id: itemIndicador.Id,
										Indicador: itemIndicador.Indicador										
									});							
						});                        
                        vm.listaPoliticasCategoriasIndicadores = vm.ListaIndicadoresAsociadosCategoriaList;
                        LimpiarChecks();
					}
				});
		}

        function buscarIndicador() {
            var campoBusqueda = document.getElementById("buscarIndicador").value;
            if (campoBusqueda == '') {
                campoBusqueda="OK"
            }
            vm.listaPoliticasCategoriasIndicadores = null;
            vm.ListaIndicadoresAsociadosCategoriaList = [];
            return modificacionLeyServicio.ConsultarCatalogoIndicadoresPolitica(vm.PoliticaSelecId, campoBusqueda).then(
                function (respuesta) {
                    var arreglo = jQuery.parseJSON(respuesta.data);
                    vm.arregloGeneral = jQuery.parseJSON(arreglo);
                    var arregloIndicadores = vm.arregloGeneral.Indicadores;
                    if (respuesta.data != null && respuesta.data != "") {
                        arregloIndicadores.forEach(itemIndicador => {                            
                            vm.ListaIndicadoresAsociadosCategoriaList.push(
                                {
                                    Id: itemIndicador.Id,
                                    Indicador: itemIndicador.Indicador
                                });
                        });
                        vm.listaPoliticasCategoriasIndicadores = vm.ListaIndicadoresAsociadosCategoriaList;
                        LimpiarChecks();
                    }
                });
        }

        function LimpiarChecks() {
            var index = 0;
            vm.listaPoliticasCategoriasIndicadores.forEach(indicador => {
                var nombreChk = "#ChckIndicador_" + index;
                $(nombreChk).prop('checked', false);;
                index++;
            });           
        }

		vm.camSeleccion = function (IndicadoresCatag) {
            if (vm.ListaIndicadoresSelc.length == 0) {
                if (IndicadoresCatag.isChecked) {
                    vm.ListaIndicadoresSelc.push({ IndicadorId: IndicadoresCatag.Id });
                }
            } else {
                if (IndicadoresCatag.isChecked) {
                    vm.ListaIndicadoresSelc = [];
                    vm.ListaIndicadoresSelc.push({ IndicadorId: IndicadoresCatag.Id });
                }
            }            			
		}

		vm.guardar = function () {
			if (vm.listaPoliticasCategoriasIndicadores.length <= 0) {
				vm.cerrar();
				return;
			}
			if (vm.ListaIndicadoresSelc.length <= 0) {
				utilidades.mensajeError("Es necesario seleccionar un indicador.", false); return false;
			}
            var IndicadorSelecionado = vm.ListaIndicadoresSelc[0].IndicadorId;
			//vm.ListaIndicadoresSelc.forEach(ItemIndicador => {			
   //             IndicadorSelecionado = ItemIndicador.IndicadorId;
			//});
            modificacionLeyServicio.GuardarModificacionesAsociarIndicadorPolitica(vm.ProyectoSelecId, vm.PoliticaSelecId, vm.CategoriaSelec, IndicadorSelecionado, "Insert").then(function (response) {
				if ((response.statusText === "OK" || response.status === 200) && response.data) {
                    var respuestaExito = response.data.Exito ;
					if (respuestaExito) {
						parent.postMessage("cerrarModal", "*");
						utilidades.mensajeSuccess("El indicador fue agregado y guardado con éxito!", false, false, "");
						vm.cerrar();
					}
					else {
						utilidades.mensajeError(respuestaMensaje, false);
						vm.cerrar();
					}
				} else {
					swal('', "Error al realizar la operación", 'error');
					vm.cerrar();
				}
			});

		}

		function funcionCancelar(reason) {
			console.log("reason", reason);
		}
	}
	angular.module('backbone').controller('modalAgregarIndicadorMlController', modalAgregarIndicadorMlController);
})();
