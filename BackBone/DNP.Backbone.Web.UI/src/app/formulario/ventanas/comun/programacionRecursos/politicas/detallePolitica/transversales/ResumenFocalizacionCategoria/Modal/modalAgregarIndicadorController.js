(function () {
	'use strict';

	modalAgregarIndicadorController.$inject = [
		'$sessionStorage',
		'$uibModalInstance',
		'utilidades',
		'$filter',
		'focalizacionAjustesServicio'
		//'politicasCategorias'
	];

	function modalAgregarIndicadorController(
		$uibModalInstance,
		$sessionStorage,
		utilidades,
		$filter,
		focalizacionAjustesServicio
		//politicasCategorias
	) {
		var vm = this;
		vm.init = init;
		vm.ConsultarPoliticasCategoriasIndicadores = ConsultarPoliticasCategoriasIndicadores;
		vm.cerrar = $sessionStorage.close;
		vm.actualizar;
		vm.habilitaFila;
		vm.Usuario = usuarioDNP;
		vm.BPIN = $uibModalInstance.idObjetoNegocio;
		vm.idInstancia = $uibModalInstance.idInstancia;
		vm.listaPoliticasCategoriasIndicadores = null;
		vm.proyectoId = $uibModalInstance.idProyectoEncabezado;
		vm.idIndicador = $uibModalInstance.idIndicador;
		vm.ListaIndicadoresSelc = [];
		vm.guardar;
		vm.CategoriaSelec = $uibModalInstance.CategoriaSelec;
		vm.focalizacionIdSelec;
		vm.nombreCategoriaSelec = $uibModalInstance.nombreCategoriaSelec;
		vm.nombreSubCategoriaSelec = $uibModalInstance.nombreSubCategoriaSelec;
		vm.nombrePoliticaSelec = $uibModalInstance.nombrePolitica;
		vm.FocalizacionId = $uibModalInstance.FocalizacionId;
		vm.indicadorSeleccionado;
		vm.categoriaId;
		vm.ListaIndicadoresAsociadosCategoriaList = [];
		vm.inhabilitar;
		vm.arregloGeneral = [];
		vm.Mensajeindicadores = "";

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

			return focalizacionAjustesServicio.ConsultarPoliticasCategoriasIndicadores(vm.BPIN, vm.Usuario, vm.idInstancia).then(
				function (respuesta) {
					var arreglo = jQuery.parseJSON(respuesta.data);

					vm.arregloGeneral = jQuery.parseJSON(arreglo);
					var arregloDatosGenerales = vm.arregloGeneral.Politicas;
					var arregloIndicadores = vm.arregloGeneral.Indicadores;
					//var ArregloCategoriasAsociadasindi = arregloIndicadores.ListadoCategoriasAsociadas;


					if (respuesta.data != null && respuesta.data != "") {
						arregloIndicadores.forEach(itemIndicador => {
							var ArregloCategoriasAsociadasindi = itemIndicador.ListadoCategoriasAsociadas;
							vm.inhabilitar = 0;
							if (itemIndicador.CategoriaId == vm.CategoriaSelec) {
								if (ArregloCategoriasAsociadasindi != null) {
									ArregloCategoriasAsociadasindi.forEach(itemcategoria => {
										if (itemcategoria.CategoriaId == itemIndicador.CategoriaId) {
											vm.inhabilitar = 1;
										}
									})
								}
								vm.ListaIndicadoresAsociadosCategoriaList.push(
									{
										IndicadorId: itemIndicador.IndicadorId,
										Indicador: itemIndicador.Indicador,
										inhabilitar: vm.inhabilitar
									});
							}
						});

						vm.listaPoliticasCategoriasIndicadores = vm.ListaIndicadoresAsociadosCategoriaList;
						if (vm.listaPoliticasCategoriasIndicadores.length > 0) {
							vm.Mensajeindicadores = "Los indicadores inactivos ya estan agregados a la categoría";
						}
						else {
							vm.Mensajeindicadores = "Esta categoría no tiene indicadores asociados";
						}
					}
				});

		}

		vm.camSeleccion = function (IndicadoresCatag) {
			if (IndicadoresCatag.isChecked) {
				vm.ListaIndicadoresSelc.push({ IndicadorId: IndicadoresCatag.IndicadorId });
			}
			else {
				var toDel = vm.ListaIndicadoresSelc.map(function (e) { return e.IndicadorId; }).indexOf(IndicadoresCatag.IndicadorId);
				vm.ListaIndicadoresSelc.splice(toDel, 1);
			}

		}

		vm.guardar = function () {
			if (vm.listaPoliticasCategoriasIndicadores.length <= 0) {
				vm.cerrar();
				return;
			}
			if (vm.ListaIndicadoresSelc.length <= 0) {
				utilidades.mensajeError("Debe Seleccionar un indicador.", false); return false;
			}
				var ArregloIndicadores = [];
				vm.ListaIndicadoresSelc.forEach(ItemIndicador => {

					let I = {
						IndicadorId: ItemIndicador.IndicadorId,
						Indicador: null,
						ProyectoId: vm.proyectoId,
						CategoriaId: vm.CategoriaSelec,
						FocalizacionIndicadorId: 0, //se envia en cero porque el post no lo usa
						Accion: "Insert"
					};
					ArregloIndicadores.push(I);
				});

				var ArregloCategorias = [{
					FocalizacionId: vm.FocalizacionId
					, ListaIndicadores: ArregloIndicadores

				}]
				var ArregloPoliticas = [{
					PoliticaId: null
					, Politica: null
					, Categorias: ArregloCategorias
				}]
				var indicadoresCategoriasGuardar = {
					ProyectoId: vm.proyectoId
					, Politicas: ArregloPoliticas

				};

				focalizacionAjustesServicio.ModificarPoliticasCategoriasIndicadores(indicadoresCategoriasGuardar, usuarioDNP, vm.idInstancia).then(function (response) {
					if ((response.statusText === "OK" || response.status === 200) && response.data) {
						var respuestaExito = JSON.parse(response.data.toString()).Exito;
						var respuestaMensaje = JSON.parse(response.data.toString()).Mensaje;
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
		angular.module('backbone').controller('modalAgregarIndicadorController', modalAgregarIndicadorController);
})();
