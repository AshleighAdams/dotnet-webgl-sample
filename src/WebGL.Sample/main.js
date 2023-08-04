import { dotnet } from './dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
	.withDiagnosticTracing(false)
	.withApplicationArgumentsFromQuery()
	.create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
const interop = exports.WebGL.Sample.Interop;

var canvas = globalThis.document.getElementById("canvas");
dotnet.instance.Module["canvas"] = canvas;

setModuleImports("main.js", {
	initialize: () => {

		var checkCanvasResize = (dispatch) => {
			var devicePixelRatio = window.devicePixelRatio || 1.0;
			var displayWidth = canvas.clientWidth * devicePixelRatio;
			var displayHeight = canvas.clientHeight * devicePixelRatio;

			if (canvas.width != displayWidth || canvas.height != displayHeight) {
				canvas.width = displayWidth;
				canvas.height = displayHeight;
				dispatch = true;
			}

			if (dispatch)
				interop.OnCanvasResize(displayWidth, displayHeight, devicePixelRatio);
		}

		function checkCanvasResizeFrame() {
			checkCanvasResize(false);
			requestAnimationFrame(checkCanvasResizeFrame);
		}

		var keyDown = (e) => {
			e.stopPropagation();
			var shift = e.shiftKey;
			var ctrl = e.ctrlKey;
			var alt = e.altKey;
			var repeat = e.repeat;
			var code = e.keyCode;

			interop.OnKeyDown(shift, ctrl, alt, repeat, code);
		}

		var keyUp = (e) => {
			e.stopPropagation();
			var shift = e.shiftKey;
			var ctrl = e.ctrlKey;
			var alt = e.altKey;
			var code = e.keyCode;

			interop.OnKeyUp(shift, ctrl, alt, code);
		}

		var mouseMove = (e) => {
			var x = e.offsetX;
			var y = e.offsetY;
			interop.OnMouseMove(x, y);
		}

		var mouseDown = (e) => {
			var shift = e.shiftKey;
			var ctrl = e.ctrlKey;
			var alt = e.altKey;
			var button = e.button;

			interop.OnMouseDown(shift, ctrl, alt, button);
		}

		var mouseUp = (e) => {
			var shift = e.shiftKey;
			var ctrl = e.ctrlKey;
			var alt = e.altKey;
			var button = e.button;

			interop.OnMouseUp(shift, ctrl, alt, button);
		}

		var shouldIgnore = (e) => {
			e.preventDefault();
			return e.touches.length > 1 || e.type == "touchend" && e.touches.length > 0;
		}

		var touchStart = (e) => {
			if (shouldIgnore(e))
				return;

			var shift = e.shiftKey;
			var ctrl = e.ctrlKey;
			var alt = e.altKey;
			var button = 0;
			var touch = e.changedTouches[0];
			var bcr = e.target.getBoundingClientRect();
			var x = touch.clientX - bcr.x;
			var y = touch.clientY - bcr.y;

			interop.OnMouseMove(x, y);
			interop.OnMouseDown(shift, ctrl, alt, button);
		}

		var touchMove = (e) => {
			if (shouldIgnore(e))
				return;

			var touch = e.changedTouches[0];
			var bcr = e.target.getBoundingClientRect();
			var x = touch.clientX - bcr.x;
			var y = touch.clientY - bcr.y;

			interop.OnMouseMove(x, y);
		}

		var touchEnd = (e) => {
			if (shouldIgnore(e))
				return;

			var shift = e.shiftKey;
			var ctrl = e.ctrlKey;
			var alt = e.altKey;
			var button = 0;
			var touch = e.changedTouches[0];
			var bcr = e.target.getBoundingClientRect();
			var x = touch.clientX - bcr.x;
			var y = touch.clientY - bcr.y;

			interop.OnMouseMove(x, y);
			interop.OnMouseUp(shift, ctrl, alt, button);
		}

		//canvas.addEventListener("contextmenu", (e) => e.preventDefault(), false);
		canvas.addEventListener("keydown", keyDown, false);
		canvas.addEventListener("keyup", keyUp, false);
		canvas.addEventListener("mousemove", mouseMove, false);
		canvas.addEventListener("mousedown", mouseDown, false);
		canvas.addEventListener("mouseup", mouseUp, false);
		canvas.addEventListener("touchstart", touchStart, false);
		canvas.addEventListener("touchmove", touchMove, false);
		canvas.addEventListener("touchend", touchEnd, false);
		checkCanvasResize(true);
		checkCanvasResizeFrame();

		canvas.tabIndex = 1000;

		interop.SetRootUri(window.location.toString());

		var langs = navigator.languages || [];
		for (var i = 0; i < langs.length; i++)
			interop.AddLocale(langs[i]);
		interop.AddLocale(navigator.language);
		interop.AddLocale(navigator.userLanguage);
	}
});

await dotnet.run();
