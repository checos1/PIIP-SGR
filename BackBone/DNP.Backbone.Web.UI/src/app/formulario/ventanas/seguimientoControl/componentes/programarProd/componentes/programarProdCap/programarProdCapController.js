(function () {
	'use strict';

	programarProdCapController.$inject = [
		'$scope',
		'$sessionStorage',
		'$uibModal',
		'utilidades',
		'desagregarEdtServicio',
		'programarProdCapServicio',
		'programarActCapServicio',
		'utilsValidacionSeccionCapitulosServicio',
		'serviciosComponenteNotificaciones'
	];

	function programarProdCapController(
		$scope,
		$sessionStorage,
		$uibModal,
		utilidades,
		desagregarEdtServicio,
		programarProdCapServicio,
		programarActCapServicio,
		utilsValidacionSeccionCapitulosServicio,
		serviciosComponenteNotificaciones

	) {
		var vm = this;
		vm.lang = "es";
		vm.pagina = 0;
		vm.nombreComponente = "programarprodprogramarprodcap";
		vm.componentesRefresh = [
			"desagregaredtdesagregarcap",
			'programaractivprogramaractcap'
		];
		vm.listadoObjProdNiveles = [];
		vm.listadoObjProdNivelesAux = [];
		vm.unidadesMedida = [];
		vm.cambiarPagina = cambiarPagina;
		vm.recorrerObjetivos = recorrerObjetivos;
		vm.recorrerObjetivosNumber = recorrerObjetivosNumber;
		vm.habilitaEditar = habilitaEditar;
		vm.Cancelar = Cancelar;
		vm.Guardar = Guardar;
		vm.Mensajes = [];
		vm.RespuestasValores = "";
		//vm.habilitaBotones = $sessionStorage.soloLectura ? false : true;
		vm.disabled = ""; 

		vm.init = function () {
			console.log("Entro programarprodprogramarprodcap");

			//vm.obtenerListadoObjProdNiveles();

			vm.disabled = $sessionStorage.soloLectura;
			vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
			vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
			vm.refreshComponente();
		};

		vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
			console.log("vm.notificacionCambiosCapitulos Programar productos", nombreCapituloHijo)
			if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
				vm.refreshComponente();
			}
		}

		vm.notificacionValidacionHijo = function (handler) {
			vm.notificacionErrores = handler;
		}

		vm.validacionGuardadoHijo = function (handler) {
			vm.validacionGuardado = handler;
		}


		vm.refreshComponente = function () {
			vm.obtenerListadoObjProdNiveles();
			//vm.obtenerParametricas();
		}
		/*
		vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
			//if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
			//	vm.init();
			//}
			console.log("vm.notificacionCambiosCapitulos Programar Productos", nombreCapituloHijo)
			if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
				vm.refreshComponente();
			}
		}
		*/
		vm.ContarTexto = function (textoLargo) {
			return textoLargo.length;
		}

		vm.CortarTexto = function (textoLargo, largo) {
			if (textoLargo.length < largo) {
				return textoLargo;
			}
			else {
				return textoLargo.substr(largo);
			}
		}

		vm.obtenerListadoObjProdNiveles = function () {
			var valor = 0;
			//var data = jQuery.parseJSON('{"ProyectoId":98117,"BPIN":"202200000000202","Objetivos":[{"ObjetivoEspecificoId":1235,"ObjetivoEspecifico":"Obtener información técnica eficiente para dar viabilidad a nuevos proyectos de construcción y ampliación de cupos","Productos":[{"ProductCatalogId":731,"ProductoId":1924,"NombreProducto":"Servicio de información penitenciaria y carcelaria para la toma de decisiones - ","Etapa":"Inversión","CostoProducto":9132000000.0000,"CostoUnitario":4566000000.0000,"DuracionPromedio":0,"FechaInicio":"2022-10-25T00:00:00","FechaFin":"2022-10-25T00:00:00","IndicadorId":2444,"IndicadorPrincipal":"Boletines de información penitenciaria y carcelaria elaborados","UnidadMedidaIndicadorPrincipal":"Número","MetaTotalIndicadorPrincipal":2.0000,"EsAcumulativoIndicadorPrincipal":false,"Vigencias":[{"PeriodoProyectoId":20005,"Vigencia":2022,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":2.0000}]},{"PeriodoProyectoId":20007,"Vigencia":2023,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000}]},{"PeriodoProyectoId":20004,"Vigencia":2024,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000}]},{"PeriodoProyectoId":20006,"Vigencia":2025,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000}]}]}]},{"ObjetivoEspecificoId":1236,"ObjetivoEspecifico":"Aumentar el número de cupos penitenciarios y carcelarios para atender a la PPL","Productos":[{"ProductCatalogId":726,"ProductoId":1923,"NombreProducto":"Infraestructura penitenciaria y carcelaria construida - ","Etapa":"Inversión","CostoProducto":716211951156.0000,"CostoUnitario":1755421448.9118,"DuracionPromedio":null,"FechaInicio":null,"FechaFin":null,"IndicadorId":2445,"IndicadorPrincipal":"Cupos penitenciarios y carcelarios entregados (nacionales y territoriales) ","UnidadMedidaIndicadorPrincipal":"Número","MetaTotalIndicadorPrincipal":408.0000,"EsAcumulativoIndicadorPrincipal":true,"Vigencias":[{"PeriodoProyectoId":20005,"Vigencia":2022,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":408.0000}]},{"PeriodoProyectoId":20007,"Vigencia":2023,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000}]},{"PeriodoProyectoId":20004,"Vigencia":2024,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000}]},{"PeriodoProyectoId":20006,"Vigencia":2025,"Meses":[{"PeriodoPeriodicidadId":1,"MesId":1,"NombreMes":"Enero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":2,"MesId":2,"NombreMes":"Febrero","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":3,"MesId":3,"NombreMes":"Marzo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":4,"MesId":4,"NombreMes":"Abril","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":5,"MesId":5,"NombreMes":"Mayo","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":6,"MesId":6,"NombreMes":"Junio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":7,"MesId":7,"NombreMes":"Julio","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":8,"MesId":8,"NombreMes":"Agosto","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":9,"MesId":9,"NombreMes":"Septiembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":10,"MesId":10,"NombreMes":"Octubre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":11,"MesId":11,"NombreMes":"Noviembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000},{"PeriodoPeriodicidadId":12,"MesId":12,"NombreMes":"Diciembre","CantidadProgramada":null,"TotalCantidadProgramada":0,"MetaTotalProgramar":0.0000}]}]}]}]}');
			//console.log("Programar Producto -> ", data);
			//vm.listadoObjProdNiveles = data;// vm.agregaValidaciones(data);

			programarProdCapServicio.obtenerListadoObjProdNiveles($sessionStorage.idObjetoNegocio, usuarioDNP)
				.then(resultado => {
					if (resultado.data != null) {
						var data = jQuery.parseJSON(jQuery.parseJSON(resultado.data));//["Objetivos"];
						vm.listadoObjProdNiveles = data;


						vm.listadoObjProdNiveles.Objetivos.forEach(obj => {
							obj.presentaError = false;
							obj.Productos.forEach(pro => {
								//pro.HabilitaEditar = $sessionStorage.soloLectura;
								pro.presentaError = false;
								var pagina = 0;
								var i = 0;
								pro.Vigencias.forEach(vig => {
									vig.Pagina = pagina;
									i++;
									if (i == 4) {
										pagina += 1;
									}
								});

								pro.Producto = pro.NombreProducto.substring(0, 90);
								if (pro.NombreProducto > 90) {
									pro.VerMas = 0;
								}
								else {
									pro.VerMas = 2;
								}

							});

							obj.Objetivo = obj.ObjetivoEspecifico.substring(0, 90);

							if (obj.ObjetivoEspecifico.length > 90) {
								obj.VerMas = 0;

							}
							else {
								obj.VerMas = 2;
							}

						});

						var valor = 0;
					}
				});
		}

		function Cancelar(productos) {
			productos.HabilitaEditar = false;
			vm.listadoObjProdNiveles = JSON.parse(vm.listadoObjProdNivelesAux);
			document.getElementById("GuardarPreguntasgrillaProdCap" + "-" + productos.ProductoId).classList.add('btnguardarDisabledDNP');
			document.getElementById("GuardarPreguntasgrillaProdCap" + "-" + productos.ProductoId).classList.remove('btnguardarDNP');
			//recorrerObjetivos(vm.datos.Objetivos);
		}
		function habilitaEditar(productos) {
			vm.listadoObjProdNivelesAux = [];
			vm.listadoObjProdNivelesAux = JSON.stringify(vm.listadoObjProdNiveles);
			productos.HabilitaEditar = true;
			document.getElementById("GuardarPreguntasgrillaProdCap" + "-" + productos.ProductoId).classList.remove('btnguardarDisabledDNP');
			document.getElementById("GuardarPreguntasgrillaProdCap" + "-" + productos.ProductoId).classList.add('btnguardarDNP');

		}

		function LimparFormulario() {
			vm.listadoObjProdNiveles.Objetivos.forEach(obj => {
				obj.Productos.forEach(pro => {
					var mensajefinal = "";
					var MetaTotalProgramar = 0;
					var mensajevalidacion = "";
					pro.Vigencias.forEach(vig => {

						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-color", "");
						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-style", "hidden");
					});
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					var Errormsn = document.getElementById("errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId);
					if (Errormsn != undefined) {
						Errormsn.innerHTML = '<span>' + "" + "</span>";
					}
				});
			});
		}

		function ValidarCantidades(pagina) {

			vm.listadoObjProdNiveles.Objetivos.forEach(obj => {
				obj.Productos.forEach(pro => {
					pro.Vigencias.forEach(vig => {

						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-color", "");
						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-style", "hidden");
					});
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					var Errormsn = document.getElementById("errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId);
					if (Errormsn != undefined) {
						Errormsn.innerHTML = '<span>' + "" + "</span>";
					}
				});
			});

			var Esporcentaje = false;

			vm.listadoObjProdNiveles.Objetivos.forEach(obj => {
				obj.Productos.forEach(pro => {
					var MetaTotalProgramar = 0;
					var mensajevalidacion = "";
					pro.Vigencias.forEach(vig => {
						if (vig.Pagina == pagina) {
							var TotalCantidadProgramada = 0;

							vig.Meses.forEach(mes => {
								if (pro.UnidadMedidaIndicadorPrincipal.indexOf("Porcentaje") != 0) {

								}
								else {
									Esporcentaje = true;
									if (mes.CantidadProgramada > 100) {
										utilidades.mensajeError("Valor no valido para porcentaje: " + mes.CantidadProgramada.replace('.',','));
										return;
									}
								}
								//if (pro.EsAcumulativoIndicadorPrincipal == true) {
								TotalCantidadProgramada += parseFloat(mes.CantidadProgramada);
								MetaTotalProgramar = mes.MetaTotalProgramar;
								//}
								//else {
								//TotalCantidadProgramada = parseFloat(mes.CantidadProgramada);
								//}
							});

							vig.TotalCantidadProgramada = TotalCantidadProgramada;

							//if (pro.UnidadMedidaIndicadorId != 15) {
							if (pro.UnidadMedidaIndicadorPrincipal.indexOf("Porcentaje") != 1) {
								if (TotalCantidadProgramada != MetaTotalProgramar) {
									mensajevalidacion += "La meta para la vigencia " + vig.Vigencia + " es " + MetaTotalProgramar + " y la programación de la cantidad de metas es " + TotalCantidadProgramada + ". Verificar las cantidades registradas. ";
									$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-color", "#AA0014");
									$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-style", "dotted");
								}
								else {
									$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-color", "");
									$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-style", "hidden");
								}
							}
							else {

							}
						}
					});

					if (Esporcentaje) {
						$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
						$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
						$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
						$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();

						$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
						$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
						$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
						$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();

						pro.VerErrormsn = 3;

					}
					else {
						if (mensajevalidacion != "") {
							$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();
							$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();

							$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();
							$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();
						}
						else {
							$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
							$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
							$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
							$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();

							$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
							$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
							$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
							$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
						}

						pro.MensajeE = mensajevalidacion.substring(0, 90);
						pro.MensajeError = mensajevalidacion;
						if (mensajevalidacion == "") {
							pro.VerErrormsn = 3;
						}
						else {
							if (mensajevalidacion.length > 90) {

								pro.VerErrormsn = 0;
							}
							else {
								pro.VerErrormsn = 2;
							}
						}
					}
				});
			});

		}

		function Guardar(productos, ObjetivoEspecificoId) {

			productos.HabilitaEditar = false;
			vm.RespuestasValores = "";

			var data = {
				ProyectoId: vm.listadoObjProdNiveles.ProyectoId,
				BPIN: vm.listadoObjProdNiveles.BPIN,
				usuarioDNP: $sessionStorage.usuario.permisos.IdUsuarioDNP,
				Objetivos: [
					{
						ObjetivoEspecificoId: ObjetivoEspecificoId,
						Productos: productos
					}
				]
			};

			vm.RespuestasValores = data;


			programarProdCapServicio.GuardarProgramarProducto(vm.RespuestasValores).then(function (response) {
				if (response.statusText === "OK" || response.status === 200) {
					setTimeout(function () {
						utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
					}, 500);

					vm.obtenerListadoObjProdNiveles();
				}
				else {
					utilidades.mensajeError(response.data.Mensaje);
				}
			});



			/*
			costoActividadesServicio.Guardar(producto, usuarioDNP)
				.then(function (response) {
					let exito = response.data;
					if (exito) {
						utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);

						vm.ObtenerResumenObjetivosProductosActividades(vm.BPIN, true);

						guardarCapituloModificado();
						return;
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
				*/
		}

		vm.agregaValidaciones = function (data) {
			// Listado de objetivos
			for (var i = 0; i < data.length; i++) {
				// Listado de productos
				var productosLength = data[i]["Productos"].length;
				data[i]["Numeracion"] = (i + 1);

				/* --- Cambia información productos-nivles --- */
				for (var j = 0; j < productosLength; j++) {
					var listadoActividadesNiveles = [];
					var listadoActividades = data[i]["Productos"][j]["Actividades"] == null ? [] : data[i]["Productos"][j]["Actividades"];
					var listadoEntregables = data[i].Productos[j]["EntregablesNivel1"];
					data[i]["Productos"][j]["Numeracion"] = (i + 1) + '.' + (j + 1);

					if (listadoEntregables != null) {
						for (var k = 0; k < listadoEntregables.length; k++) {
							// Validaciones Nivel 1
							var catalogoEntregableProducto = data[i].Productos[j].EntregablesNivel1[k]["CatalogoEntregables"];
							var nivelesRegistrados = data[i].Productos[j].EntregablesNivel1[k]["NivelesRegistrados"];
							data[i].Productos[j].EntregablesNivel1[k]["Numeracion"] = (i + 1) + '.' + (j + 1) + '.' + (k + 1);
							data[i].Productos[j].EntregablesNivel1[k]["DataAgregarModal"] = {
								Nivel: "Nivel 2",
								NivelPadre: "Nivel 1",
								Producto: data[i]["Productos"][j],
								PadreId: null,
								Hijos: data[i].Productos[j].EntregablesNivel1[k]["NivelesRegistrados"],
								Data: data[i].Productos[j].EntregablesNivel1[k],
								ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
								CatalogoEntregables: catalogoEntregableProducto,
								ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
								Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"]
							};

							for (var l = 0; l < nivelesRegistrados.length; l++) {
								// Validaciones Nivel 2 
								if (nivelesRegistrados[l].NivelEntregable == "Actividad") listadoActividadesNiveles.push(NivelEntregable[l])
								var hijoNivel2 = data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["Hijos"];
								data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["DataAgregarModal"] = {
									Nivel: "Nivel 3",
									NivelPadre: "Nivel 2",
									Producto: data[i]["Productos"][j],
									PadreId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["SeguimientoEntregableId"],
									Hijos: hijoNivel2,
									Data: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l],
									ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
									CatalogoEntregables: catalogoEntregableProducto,
									ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
									Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
									Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"]
								};
								for (var m = 0; m < hijoNivel2.length; m++) {
									// Validaciones Nivel 3
									if (hijoNivel2[m].NivelEntregable == "Actividad") listadoActividadesNiveles.push(hijoNivel2[m])
									var hijoNivel3 = hijoNivel2[m]["Hijos"];
									data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["DataAgregarModal"] = {
										Nivel: "Actividad",
										NivelPadre: "Nivel 3",
										Producto: data[i]["Productos"][j],
										PadreId: hijoNivel2[m]["SeguimientoEntregableId"],
										Hijos: hijoNivel3,
										Data: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos,
										ActividadId: data[i].Productos[j].EntregablesNivel1[k]["ActividadId"],
										CatalogoEntregables: catalogoEntregableProducto,
										ProductoCollapseId: data[i].Productos[j]["IdCompuesto"],
										Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
										Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"],
										Nivel3CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["IdCompuesto"],
									};
									for (var n = 0; n < hijoNivel3.length; n++) {
										// Validaciones Actividades de Nivel 3
										if (hijoNivel3[n].NivelEntregable == "Actividad") listadoActividadesNiveles.push(hijoNivel3[n])
										data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["Hijos"][n]["DataAgregarModal"] = {
											Producto: data[i]["Productos"][j],
											Nivel1CollapseId: data[i].Productos[j].EntregablesNivel1[k]["IdCompuesto"],
											Nivel2CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l]["IdCompuesto"],
											Nivel3CollapseId: data[i].Productos[j].EntregablesNivel1[k].NivelesRegistrados[l].Hijos[m]["IdCompuesto"],
										};
									}
								}
							}
						}
					}

					/* --- Agrega Actividades de todos los niveles a productos --- */
					data[i]["Productos"][j]["ActividadesNiveles"] = [...listadoActividadesNiveles];
					data[i]["Productos"][j]["ActividadesNiveles"].concat(listadoActividades);
					console.log("Producto ActividadesNiveles", i, data[i]["Productos"][j].ActividadesNiveles)
				}
			}
			return data;
		}

		/*--------------------- Comportamientos collapse y contenido ---------------------*/

		vm.verNombreCompleto = function (ObjetivoEspecificoId, indexElement, objetivo) {

			vm.listadoObjProdNiveles.Objetivos.forEach(obj => {

				if (objetivo == 0) {
					obj.Productos.forEach(pro => {
						if (pro.ProductoId == ObjetivoEspecificoId) {
							if (indexElement == 0) {
								pro.VerErrormsn = 1;

							}
							else {
								pro.VerErrormsn = 0;
							}
						}
					});
				}
				else if (objetivo == 1) {
					if (obj.ObjetivoEspecificoId == ObjetivoEspecificoId) {

						if (indexElement == 0) {
							obj.VerMas = 1;

						}
						else {
							obj.VerMas = 0;
						}
					}
				}
				else {
					obj.Productos.forEach(pro => {
						if (pro.ProductoId == ObjetivoEspecificoId) {
							if (indexElement == 0) {
								pro.VerErrormsn = 1;

							}
							else {
								pro.VerErrormsn = 0;
							}
						}
					});
				}

			});

			/*
			var elValidacion = document.getElementById(idElement + indexElement + '-val');
			var elCorto = document.getElementById(idElement + indexElement + '-min');
			var elCompleto = document.getElementById(idElement + indexElement + '-max');

			if (elCompleto.classList.contains('hidden')) {
				elValidacion.innerHTML = 'VER MENOS';
				elCorto.classList.add('hidden');
				elCompleto.classList.remove('hidden');
			} else {
				elValidacion.innerHTML = 'VER MÁS';
				elCorto.classList.remove('hidden');
				elCompleto.classList.add('hidden');
			}
			*/
		}

		vm.abrilNivel = function (idElement) {
			idElement = idElement.replace("null", "");
			var elMas = document.getElementById(idElement + '-mas');
			var elMenos = document.getElementById(idElement + '-menos');

			if (elMas != null && elMenos != null) {
				if (elMas.classList.contains('hidden')) {
					elMenos.classList.add('hidden');
					elMas.classList.remove('hidden');
				} else {
					elMenos.classList.remove('hidden');
					elMas.classList.add('hidden');
				}
			}
			setTimeout(function () {
				LimparFormulario();

			}, 500);
			setTimeout(function () {
				ValidarCantidades(vm.pagina);
			}, 500);

		}

		vm.abrirMensajeQueEsEsto = function () {
			utilidades.mensajeInformacionN("", null, "<span class='anttituhori' > ¿Qué es esto? </span><br /> <br /> <span class='tituhori' >Programar Actividades - Desglose de actividades</span>");
		}

		function cambiarPagina(index) {
			vm.pagina = index;
			var pagina = 0;
			if (index == 4)
				pagina = 1;
			if (index == 8)
				pagina = 2;
			if (index == 12)
				pagina = 3;
			if (index == 16)
				pagina = 4;
			if (index == 20)
				pagina = 5;

			vm.listadoObjProdNiveles.Objetivos.forEach(obj => {
				obj.Productos.forEach(pro => {
					pro.Vigencias.forEach(vig => {

						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-color", "");
						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-style", "hidden");
					});
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					var Errormsn = document.getElementById("errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId);
					if (Errormsn != undefined) {
						Errormsn.innerHTML = '<span>' + "" + "</span>";
					}
				});
			});

			//setTimeout(function () {
			//	LimparFormulario();
			//}, 500);
			setTimeout(function () {
				ValidarCantidades(pagina);
			}, 500);


		}

		function recorrerObjetivosNumber(event, producto, Vigencia, pagina) {

			if (Number.isNaN(event.target.value)) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
			}

			if (event.target.value == null) {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
			}

			if (event.target.value == "") {
				return new Intl.NumberFormat('es-co', { minimumFractionDigits: 4, }).format(0.0000);
			}

			event.target.value = event.target.value.replace(",", ".");

			setTimeout(function () {
				LimparFormulario();

			}, 500);
			setTimeout(function () {
				ValidarCantidades(pagina);
			}, 500);

			//recorrerObjetivos(producto, Vigencia);

		}


		function recorrerObjetivos(producto, Vigencia) {
			var mensajefinal = "";
			var TotalCantidadProgramada = 0;
			var MetaTotalProgramar = 0;
			var mensajevalidacion = "";



			producto.Vigencias.forEach(vig => {
				if (vig.Vigencia == Vigencia) {
					vig.Meses.forEach(mes => {
						TotalCantidadProgramada += mes.CantidadProgramada;
						MetaTotalProgramar = mes.MetaTotalProgramar;
					})
					vig.TotalCantidadProgramada = TotalCantidadProgramada;



					if (producto.UnidadMedidaIndicadorId != 15) {
						if (TotalCantidadProgramada > MetaTotalProgramar) {
							mensajevalidacion += "La meta para la vigencvia " + Vigencia + " es " + MetaTotalProgramar + " y la programación de la cantidad de metas es " + TotalCantidadProgramada + ". Verificar las cantidades registradas. ";
							$("#total" + producto.ProductoId + Vigencia).css("border-color", "red");
							$("#total" + producto.ProductoId + Vigencia).css("border-style", "solid");
						}
						else {
							$("#total" + producto.ProductoId + Vigencia).css("border-color", "");
							$("#total" + producto.ProductoId + Vigencia).css("border-style", "hidden");
						}

						if (mensajevalidacion != "") {
							$("#imgcptjpp" + producto.ProductoId).attr('disabled', false);
							$("#imgcptjpp" + producto.ProductoId).fadeIn();
							$("#icocptjpp" + producto.ProductoId).attr('disabled', false);
							$("#icocptjpp" + producto.ProductoId).fadeIn();
						}
						else {
							$("#imgcptjpp" + producto.ProductoId).attr('disabled', true);
							$("#imgcptjpp" + producto.ProductoId).fadeOut();
							$("#icocptjpp" + producto.ProductoId).attr('disabled', true);
							$("#icocptjpp" + producto.ProductoId).fadeOut();
						}
					}
				}
			});
			mensajefinal += mensajevalidacion;



		}
		function recorrerProductos(Productos) {
			var valor = 0;
		}

		vm.validateFormat = function (event, cantidad) {

			if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
				event.preventDefault();
			}

			var newValue = event.target.value;
			var spiltArray = String(newValue).split("");
			var tamanioPermitido = 12;
			var tamanio = event.target.value.length;
			var permitido = false;
			permitido = event.target.value.toString().includes(".");
			if (permitido) {
				tamanioPermitido = 16;

				var n = String(newValue).split(".");
				if (n[1]) {
					var n2 = n[1].slice(0, cantidad);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > cantidad) {
						tamanioPermitido = n[0].length + cantidad;
						event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
						return;
					}

					if ((n[1].length == 2 && n[1] > 99) || (n[1].length > 3 && n[1] > 9999)) {
						event.preventDefault();
					}
				}
			} else {
				if (tamanio > 15 && event.keyCode != 44) {
					event.preventDefault();
				}
			}

			if (event.keyCode === 44 && tamanio == 15) {
			}
			else {
				if (tamanio > tamanioPermitido || tamanio > 15) {
					event.preventDefault();
				}
			}
		}

		vm.validarTamanio = function (event, cantidad) {
			var regexp = /^\d+\.\d{0,2}$/;
			var valida = regexp.test(event.target.value);

			if (Number.isNaN(event.target.value)) {
				event.target.value = "0"
				return;
			}

			if (event.target.value == null) {
				event.target.value = "0"
				return;
			}

			if (event.target.value == "") {
				event.target.value = "0"
				return;
			}

			var newValue = event.target.value;
			var spiltArray = String(newValue).split("");
			var tamanioPermitido = 11;
			var tamanio = event.target.value.length;
			var permitido = false;
			permitido = event.target.value.toString().includes(".");
			if (permitido) {
				var indicePunto = event.target.value.toString().indexOf(".");
				var decimales = event.target.value.toString().substring(indicePunto, tamanio).length;
				if (decimales > cantidad) {
				}
				tamanioPermitido = 16;

				var n = String(newValue).split(".");
				if (n[1]) {
					var n2 = n[1].slice(0, cantidad);
					newValue = [n[0], n2].join(".");
					if (spiltArray.length === 0) return;
					if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
					if (spiltArray.length === 2 && newValue === '-.') return;

					if (n[1].length > 4) {
						tamanioPermitido = n[0].length + cantidad;
						event.target.value = n[0] + '.' + n[1].slice(0, cantidad);
						return;
					}

					if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
						event.preventDefault();
					}
				}
			}
		}

		/*--------------------- Agregar - Eliminar niveles / actividades ---------------------*/

		vm.abrirArbol = function (dataModal) {
			var arbolCollapse = {
				productoId: "productos-nivel-1-" + dataModal["Producto"].ProductoId,
				productoIdCompuesto: dataModal["Producto"].IdCompuesto,
				entregablesNvl1: "items-" + dataModal["Nivel1CollapseId"],
				entregablesNvl2: "items-" + dataModal["Nivel2CollapseId"],
				entregablesNvl3: "items-" + dataModal["Nivel3CollapseId"],
			}
			vm.obtenerListadoObjProdNiveles(arbolCollapse);
		}

		/*--------------------- Validaciones ---------------------*/

		vm.notificacionValidacionPadre = function (errores) {

			console.log("Validación  - Programar producto");
			vm.limpiarErrores();
			if (errores != undefined) {
				vm.erroresActivos = [];
				var erroresFiltrados = utilsValidacionSeccionCapitulosServicio.getErroresValidados(vm.nombreComponente, errores);
				vm.erroresActivos = erroresFiltrados.erroresActivos;
				vm.ejecutarErrores();
				vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: erroresFiltrados.isValid });
			}
		};

		vm.limpiarErrores = function () {
			vm.listadoObjProdNiveles.Objetivos.forEach(obj => {
				obj.presentaError = false;
				obj.Productos.forEach(pro => {
					pro.presentaError = false;
					pro.Vigencias.forEach(vig => {

						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-color", "");
						$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-style", "hidden");
					});
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', true);
					$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeOut();
					var Errormsn = document.getElementById("errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId);
					if (Errormsn != undefined) {
						Errormsn.innerHTML = '<span>' + "" + "</span>";
					}
				});
			});
		}

		vm.ejecutarErrores = function () {
			vm.limpiarErrores();
			vm.erroresActivos.forEach(p => {
				if (vm.errores[p.Error] != undefined) {
					vm.errores[p.Error]({
						error: p.Error,
						descripcion: p.Descripcion,
						data: p.Data == undefined ? '' : p.Data
					});
				}
			});
		}

		vm.validarErroresProgramarProducto = function (errores) {
			vm.listadoObjProdNiveles.Objetivos.forEach(obj => {
				var mensajevalidacion = "";
				obj.Productos.forEach(pro => {
					try {
						var erroresnun = errores.descripcion.split(';');
					}
					catch {
						var erroresnun = [];
						erroresnun.push(errores.descripcion);
					}
					erroresnun.forEach(erro => {
						var erroresesp = erro.split('|');
						var objid = erroresesp[0];
						var proid = erroresesp[1];
						var vige = erroresesp[2];
						var err = erroresesp[3];


						if (obj.ObjetivoEspecificoId == objid && pro.ProductoId == proid) {

							obj.presentaError = true;
							pro.presentaError = true;
							pro.Vigencias.forEach(vig => {
								if (vig.Vigencia == vige) {

									if (pro.UnidadMedidaIndicadorPrincipal.indexOf("Porcentaje") != 1) {
										mensajevalidacion += err;
										$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-color", "#AA0014");
										$("#total" + obj.ObjetivoEspecificoId + pro.ProductoId + vig.Vigencia).css("border-style", "dotted");
									}
								}
							});

							$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#errortottalpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();
							$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#errortottalmsnpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();

							$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#imgcptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();
							$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).attr('disabled', false);
							$("#icocptjpp" + obj.ObjetivoEspecificoId + pro.ProductoId).fadeIn();

							pro.MensajeE = mensajevalidacion.substring(0, 90);
							pro.MensajeError = mensajevalidacion;
							if (mensajevalidacion == "") {
								pro.VerErrormsn = 3;
							}
							else {
								if (mensajevalidacion.length > 90) {

									pro.VerErrormsn = 0;
								}
								else {
									pro.VerErrormsn = 2;
								}
							}
						}

					});
				});
			});
		}


		vm.errores = {
			'PROPRO': vm.validarErroresProgramarProducto,
		}

		/*
		vm.notificacionValidacion = function (errores) {
			console.log("Validación  - Justificación Categorías políticas transversales");
			vm.limpiarErrores();
			var isValid = true;
			if (errores != undefined) {
				var erroresfocalizacionpt = errores.find(p => p.Capitulo == "focalizacionpolresumendefocali");
				if (erroresfocalizacionpt != undefined) {
					var erroresJson = erroresfocalizacionpt.Errores == "" ? [] : JSON.parse(erroresfocalizacionpt.Errores);
					var isValid = (erroresJson == null || erroresJson.length == 0);
					if (!isValid) {
						erroresJson.errores.forEach(p => {
							if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
						});
					}
	
				}
			}
			vm.ejecutarErrores();
			vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
		};
		*/

	}

	angular.module('backbone').component('programarProdCap', {
		templateUrl: "src/app/formulario/ventanas/seguimientoControl/componentes/programarProd/componentes/programarProdCap/programarProdCap.html",
		controller: programarProdCapController,
		controllerAs: "vm",
		bindings: {
			bpin: '@',
			guardadoevent: '&',
			notificacionvalidacion: '&',
			notificacioncambios: '&',
			notificacionestado: '&'
		}
	});

})();