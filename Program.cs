using System.Text;
using System.Text.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http.Features;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);
const long MaxVideoBytes = 250L * 1024L * 1024L;
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = MaxVideoBytes + (2L * 1024L * 1024L));
builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = MaxVideoBytes + (2L * 1024L * 1024L));
var app = builder.Build();

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="es">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta name="theme-color" content="#06143b">
<title>MARY | Tu estudio creativo con IA</title>
<style>
:root{--navy:#06143b;--navy2:#081d50;--blue:#1263ee;--cyan:#28d7ff;--ink:#102451;--muted:#64748b;--line:#dce8fb;--paper:#f7faff}
*{box-sizing:border-box}
html{scroll-behavior:smooth}
body{margin:0;font-family:Inter,Segoe UI,Arial,sans-serif;background:var(--navy);color:#fff}
button,input,select,textarea{font:inherit}
button{cursor:pointer}
.site-header{position:sticky;top:0;z-index:50;background:rgba(4,15,43,.94);backdrop-filter:blur(16px);border-bottom:1px solid rgba(83,164,255,.24)}
.header-inner{max-width:1440px;margin:auto;min-height:76px;padding:12px 28px;display:flex;align-items:center;gap:28px}
.brand{display:flex;align-items:center;gap:10px;text-decoration:none;color:white;font-size:27px;font-weight:900;letter-spacing:1.5px;white-space:nowrap}
.brand-mark{display:grid;place-items:center;width:42px;height:42px;border-radius:14px;background:linear-gradient(145deg,#27e4ff,#1263ee 58%,#0b2f91);box-shadow:0 0 22px #168cff88;color:white;font-size:24px;font-weight:900}
.main-nav{display:flex;align-items:center;gap:8px;margin-left:auto}
.main-nav a{color:#d9e8ff;text-decoration:none;font-size:14px;padding:11px 14px;border-radius:11px;transition:.2s}
.main-nav a:hover,.main-nav a.current{background:#1559c9;color:#fff;box-shadow:0 5px 18px #0757db55}
.header-cta{border:1px solid #63dfff;background:linear-gradient(110deg,#0759db,#1748bd);color:#fff;padding:12px 18px;border-radius:30px;font-weight:800;box-shadow:0 0 20px #008cff55;white-space:nowrap}
.mobile-toggle{display:none;margin-left:auto;border:1px solid #2d65b8;background:#102b63;color:white;border-radius:10px;padding:10px 13px}
.page{max-width:1440px;margin:auto;padding:26px 28px 48px}
.hero{position:relative;overflow:hidden;min-height:390px;padding:42px 48px;border-radius:28px;background:radial-gradient(circle at 14% 85%,#0879ff 0,transparent 30%),radial-gradient(circle at 88% 20%,#114fcb 0,transparent 33%),linear-gradient(115deg,#061a4e 0%,#073a9b 53%,#06183f 100%);border:1px solid #2c91ff;box-shadow:0 18px 55px #001b58aa,inset 0 0 45px #1d70ff30;display:grid;grid-template-columns:minmax(0,1fr) 330px;align-items:center;gap:20px}
.hero:before,.hero:after{content:"";position:absolute;border:1px solid #56d8ff55;border-radius:50%;pointer-events:none}
.hero:before{width:530px;height:530px;right:-100px;top:-300px;box-shadow:0 0 45px #1b9dff33}
.hero:after{width:380px;height:380px;left:-230px;bottom:-280px}
.hero-copy,.hero-art{position:relative;z-index:1}
.pill{display:inline-flex;align-items:center;gap:8px;padding:8px 13px;border:1px solid #8aeaff88;border-radius:30px;background:#ffffff17;color:#d8f7ff;font-size:12px;font-weight:800;letter-spacing:1.2px}
.hero h1{font-size:clamp(58px,8vw,100px);line-height:.94;letter-spacing:2px;margin:22px 0 13px;font-weight:950;text-shadow:0 5px 0 #093c93,0 0 26px #29caff88}
.hero-subtitle{display:inline-block;padding:10px 18px;border:1px solid #8be9ff;border-radius:30px;background:linear-gradient(90deg,#075ce4,#1743bc);font-size:clamp(15px,2vw,21px);font-weight:800;box-shadow:0 0 20px #29c7ff66}
.hero-tagline{margin:18px 0 0;color:#fff;font-size:clamp(18px,2.5vw,27px);font-style:italic;font-weight:700}
.hero-tagline span{color:#ffe66b}
.hero-description{max-width:640px;color:#dceaff;line-height:1.7;margin:13px 0 0;font-size:14px}
.hero-art{height:275px;display:grid;place-items:center}
.robot{position:relative;width:230px;height:245px;filter:drop-shadow(0 0 22px #20cfff9c)}
.robot-head{position:absolute;top:9px;left:31px;width:168px;height:142px;border-radius:48% 48% 42% 42%;background:linear-gradient(145deg,#fff,#c4e4ff 47%,#6a9bd1);border:5px solid #b4eaff;box-shadow:inset -10px -10px 15px #3269a8,0 0 0 5px #0d54ca,0 0 30px #23d7ff}
.robot-face{position:absolute;inset:17px 13px 20px;border-radius:42%;background:linear-gradient(145deg,#06143a,#0c2861);display:flex;align-items:center;justify-content:center;gap:22px;box-shadow:inset 0 0 19px #1a7cff}
.robot-eye{width:17px;height:27px;border-radius:50%;background:#27e5ff;box-shadow:0 0 13px #27e5ff}
.robot-smile{position:absolute;width:39px;height:17px;border-bottom:4px solid #26dcff;border-radius:0 0 30px 30px;bottom:21px}
.robot-ear{position:absolute;top:48px;width:23px;height:52px;border-radius:10px;background:linear-gradient(90deg,#5ba8e9,#f6fcff);border:3px solid #c6f2ff}
.robot-ear.left{left:18px}.robot-ear.right{right:18px}
.robot-body{position:absolute;bottom:0;left:57px;width:116px;height:103px;border-radius:35px 35px 22px 22px;background:linear-gradient(145deg,#f7fdff,#78b7ee 65%,#2365b7);border:4px solid #9eeaff;box-shadow:inset -8px -8px 13px #164b91}
.robot-badge{position:absolute;left:85px;bottom:23px;width:60px;height:51px;border-radius:15px;background:linear-gradient(145deg,#0748c2,#061d66);border:2px solid #26ddff;display:grid;place-items:center;color:#fff;font-size:30px;font-weight:950;text-shadow:0 0 10px #25dfff}
.float-icon{position:absolute;display:grid;place-items:center;width:49px;height:49px;border-radius:15px;background:linear-gradient(145deg,#fff,#d6ecff);color:#0755c9;font-size:25px;box-shadow:0 0 20px #18cfff88;border:1px solid #7ee9ff}
.float-icon.one{right:3px;top:24px}.float-icon.two{left:0;bottom:45px}.float-icon.three{right:0;bottom:20px}
.section-heading{display:flex;justify-content:space-between;align-items:end;gap:15px;margin:34px 3px 17px}
.section-heading h2{margin:0;font-size:25px;letter-spacing:-.4px}.section-heading p{margin:6px 0 0;color:#b8d3ff;font-size:13px}
.section-kicker{color:#55dcff;font-size:11px;font-weight:900;letter-spacing:1.8px;text-transform:uppercase;margin-bottom:8px}
.tools-grid{display:grid;grid-template-columns:repeat(2,minmax(0,1fr));gap:20px}
.tool-card{position:relative;min-height:210px;border:1px solid #d0e6ff;border-radius:24px;background:linear-gradient(135deg,#fff 0%,#edf5ff 72%,#dceaff 100%);color:var(--ink);padding:23px;display:grid;grid-template-columns:135px minmax(0,1fr) auto;align-items:center;gap:18px;text-align:left;box-shadow:0 9px 30px #001a4f55,inset 0 1px 0 #fff;transition:transform .2s,box-shadow .2s}
.tool-card:hover{transform:translateY(-5px);box-shadow:0 17px 35px #0078ff44,0 0 0 2px #58cfff}
.tool-icon{width:126px;height:126px;border-radius:50%;display:grid;place-items:center;font-size:54px;color:white;background:radial-gradient(circle at 33% 26%,#3098ff,#0757dc 56%,#05256e 100%);border:5px solid #9bdfff;box-shadow:0 0 0 4px #d9f4ff,0 0 25px #168fff77,inset 0 0 18px #8deaff66}
.tool-copy h3{font-size:23px;line-height:1.15;margin:0 0 9px;font-weight:900;letter-spacing:-.5px}
.tool-copy p{font-size:13px;color:#435b80;line-height:1.55;margin:0 0 12px}
.tool-points{display:flex;flex-wrap:wrap;gap:7px 12px;color:#1c4d91;font-size:11px;font-weight:700}
.tool-points span:before{content:"✓";display:inline-grid;place-items:center;margin-right:5px;width:16px;height:16px;border-radius:50%;background:#1263ee;color:white;font-size:10px}
.tool-action{align-self:start;margin-top:14px;border:0;border-radius:30px;background:linear-gradient(110deg,#0875f9,#103eb8);padding:13px 16px;color:#fff;font-size:13px;font-weight:850;white-space:nowrap;box-shadow:0 7px 16px #1456bd38}
.tool-action:hover{filter:brightness(1.1)}
.benefits{display:grid;grid-template-columns:repeat(4,1fr);gap:10px;margin:28px 3px 0;padding:17px 20px;border:1px solid #2f9cff;border-radius:20px;background:linear-gradient(100deg,#09245e,#06143b);box-shadow:0 0 24px #0759d744}
.benefit{display:flex;align-items:center;justify-content:center;gap:11px;color:#e8f4ff;font-weight:700;font-size:13px;border-right:1px solid #4c82c8;padding:5px 12px}
.benefit:last-child{border:0}.benefit b{font-size:25px;color:#36d9ff}
.bottom-cta{display:flex;align-items:center;justify-content:center;gap:18px;text-align:center;padding:33px 10px 9px;color:#f3f8ff;font-size:clamp(18px,2.5vw,27px);font-style:italic;font-weight:750}
.bottom-cta span{color:#ffe35e}
.cta-button{background:linear-gradient(110deg,#086dff,#1038b3);border:1px solid #78e8ff;color:white;border-radius:30px;padding:14px 22px;font-size:14px;font-weight:850;box-shadow:0 0 20px #058cff77;font-style:normal}
.work-area{margin-top:35px;scroll-margin-top:95px}
.tool-panel{display:none;background:#fff;color:var(--ink);border-radius:22px;padding:26px;border:1px solid #d7e7fb;box-shadow:0 12px 35px #00174455}
.tool-panel.visible{display:block;animation:appear .25s ease}
@keyframes appear{from{opacity:0;transform:translateY(8px)}to{opacity:1;transform:translateY(0)}}
.subtool-grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:12px;margin:20px 0 22px}
.subtool-card{display:flex;flex-direction:column;align-items:flex-start;gap:7px;min-height:132px;padding:16px;border:1px solid #d8e7fa;border-radius:16px;background:linear-gradient(145deg,#fafdff,#eaf3ff);color:#15335f;text-align:left;transition:.18s}
.subtool-card:hover{transform:translateY(-3px);border-color:#60aaff;box-shadow:0 9px 20px #1456bd1b}
.subtool-card>span{display:grid;place-items:center;width:39px;height:39px;border-radius:12px;background:#d9eaff;color:#1456b8;font-size:22px}
.subtool-card strong{font-size:14px}.subtool-card small{font-size:11px;line-height:1.45;color:#58708f}
.nested-workspace{align-items:start}.nested-box{min-width:0;padding:18px;border:1px solid #e0eaf7;border-radius:17px;background:#fbfdff}
.nested-box h3{margin:0 0 6px;color:#173763;font-size:17px}.nested-box>p{margin:0 0 13px;color:#687e9c;font-size:12px;line-height:1.55}
.panel-head{display:flex;align-items:flex-start;justify-content:space-between;gap:18px;margin-bottom:18px}
.panel-head h2{margin:0 0 5px;font-size:24px}.panel-head p{margin:0;color:#64748b;font-size:13px;line-height:1.5}
.close-panel{border:0;border-radius:10px;background:#eaf2ff;color:#174da0;padding:9px 12px;font-weight:800}
.field-label{display:block;font-size:13px;font-weight:850;margin:16px 0 8px;color:#18345f}
.field,select,textarea{width:100%;padding:13px 14px;border:1px solid #d7e2f2;border-radius:12px;background:#f8fbff;color:#142b50;outline:none}
.field:focus,select:focus,textarea:focus{border-color:#3185f5;box-shadow:0 0 0 3px #2b8bff1b}
textarea{min-height:110px;resize:vertical}
.primary-button{border:0;border-radius:12px;padding:13px 18px;background:linear-gradient(100deg,#0875f9,#1742bb);color:#fff;font-weight:850;box-shadow:0 7px 15px #1657bc30}
.secondary-button{border:1px solid #c5d9f4;border-radius:12px;padding:12px 16px;background:#edf5ff;color:#1754a4;font-weight:800}
.field-help{font-size:12px;color:#64748b;line-height:1.5;margin:7px 0 0}.consent-row{display:flex;align-items:flex-start;gap:10px;padding:12px;border:1px solid #bfd7f5;border-radius:12px;background:#f0f6ff;color:#193b69;font-size:13px}.consent-row input{margin-top:2px}
.panel-note{margin-top:15px;padding:12px 14px;border-radius:12px;background:#eff6ff;color:#48617f;font-size:12px;line-height:1.55}
.panel-grid{display:grid;grid-template-columns:repeat(2,minmax(0,1fr));gap:16px}
.radio-row{display:flex;gap:10px;flex-wrap:wrap}.radio-pill{display:flex;align-items:center;gap:7px;background:#f1f6ff;border:1px solid #d8e6f8;border-radius:11px;padding:11px 13px;font-size:13px;color:#23456f}
.output{display:none;margin-top:17px;padding:17px;border:1px solid #d9e8fa;background:#f3f7ff;border-radius:13px;white-space:pre-wrap;line-height:1.65;overflow-wrap:anywhere;color:#20395f}
.output.visible{display:block}
.output button{margin-top:12px}
.download-link{display:inline-block;margin-top:12px;padding:11px 16px;border-radius:10px;background:#1263ee;color:#fff;text-decoration:none;font-weight:850}
.clip-downloads{display:flex;flex-direction:column;align-items:flex-start;gap:8px;margin-top:10px}.clip-downloads .download-link{display:block;max-width:100%;overflow-wrap:anywhere}
.clip-result-item{width:100%;padding:12px;border:1px solid #cbdcf3;border-radius:12px;background:#fff}.clip-result-detail{margin-bottom:8px;color:#23456f;font-size:12px;line-height:1.55}
.image-results{display:grid;grid-template-columns:repeat(4,minmax(0,1fr));gap:13px;margin-top:18px}
.image-card{overflow:hidden;border:1px solid #dce7f5;border-radius:13px;background:#fff}
.image-card img{display:block;width:100%;height:150px;object-fit:cover;background:#eaf1fa}
.image-meta{padding:10px;font-size:11px;color:#526781;line-height:1.4;overflow-wrap:anywhere}
.image-meta strong{display:block;color:#19345b;margin-bottom:5px}
.image-meta a{color:#075dc9;text-decoration:none;font-weight:800}
.generator{margin-top:30px;background:#fff;color:var(--ink);border-radius:22px;padding:25px;border:1px solid #d7e7fb;box-shadow:0 12px 35px #00174455;scroll-margin-top:95px}
.generator h2{margin:0 0 7px;font-size:23px}.generator>p{margin:0;color:#64748b;font-size:13px}
.generator-actions{display:flex;gap:10px;flex-wrap:wrap}
.library{margin-top:25px;background:#fff;color:var(--ink);border:1px solid #d7e7fb;border-radius:20px;padding:22px;scroll-margin-top:95px}
.library h2{margin:0 0 15px;font-size:21px}
.saved-item{padding:13px 0;border-bottom:1px solid #e4edf8;font-size:13px;line-height:1.55;overflow-wrap:anywhere}
.saved-item button{margin:8px 8px 0 0;padding:8px 11px;border:0;border-radius:8px;background:#e3efff;color:#174da0;font-weight:750}
.empty{color:#6b7c95;font-size:13px}
.site-footer{max-width:1440px;margin:auto;padding:0 28px 26px;color:#91b7e8;font-size:11px;text-align:center}
@media(max-width:1120px){.tool-card{grid-template-columns:105px minmax(0,1fr);gap:15px}.tool-icon{width:100px;height:100px;font-size:45px}.tool-action{grid-column:2;justify-self:start;margin-top:0}.tool-copy h3{font-size:20px}}
@media(max-width:820px){.header-inner{padding:11px 16px}.main-nav{display:none;position:absolute;top:68px;left:12px;right:12px;padding:10px;background:#071a49;border:1px solid #245ba0;border-radius:14px;flex-direction:column;align-items:stretch}.main-nav.open{display:flex}.main-nav a{padding:13px}.header-cta{display:none}.mobile-toggle{display:block}.page{padding:18px 16px 35px}.hero{grid-template-columns:1fr;padding:30px 25px;min-height:0}.hero-art{position:absolute;right:15px;bottom:5px;transform:scale(.72);transform-origin:bottom right;opacity:.7}.hero-copy{max-width:75%;z-index:2}.hero h1{font-size:65px}.hero-subtitle{font-size:14px}.hero-description{font-size:13px}.tools-grid{gap:13px}.tool-card{min-height:185px;padding:17px;grid-template-columns:80px minmax(0,1fr)}.tool-icon{width:76px;height:76px;font-size:34px;border-width:3px}.tool-copy h3{font-size:18px}.tool-copy p{font-size:12px}.tool-points{font-size:10px}.benefits{grid-template-columns:repeat(2,1fr)}.benefit:nth-child(2){border-right:0}.benefit:nth-child(3),.benefit:nth-child(4){border-top:1px solid #4c82c8}.image-results{grid-template-columns:repeat(3,minmax(0,1fr))}}
@media(max-width:560px){.brand{font-size:22px}.brand-mark{width:37px;height:37px}.hero{padding:27px 20px}.hero-copy{max-width:100%}.hero h1{font-size:58px;margin-top:17px}.hero-art{opacity:.15;right:-25px;bottom:-20px;transform:scale(.8)}.hero-description{max-width:92%}.section-heading h2{font-size:21px}.tools-grid{grid-template-columns:1fr}.tool-card{min-height:160px;grid-template-columns:82px minmax(0,1fr);padding:16px}.tool-icon{width:78px;height:78px}.tool-action{grid-column:2}.benefits{padding:12px 8px}.benefit{font-size:11px;padding:9px 4px;justify-content:flex-start}.benefit b{font-size:21px}.bottom-cta{flex-direction:column;padding-top:25px}.panel-grid{grid-template-columns:1fr}.subtool-grid{grid-template-columns:repeat(2,minmax(0,1fr))}.tool-panel,.generator{padding:19px}.image-results{grid-template-columns:repeat(2,minmax(0,1fr))}.image-card img{height:130px}.panel-head{gap:8px}.panel-head h2{font-size:20px}}
</style>
</head>
<body>
<header class="site-header">
  <div class="header-inner">
    <a class="brand" href="#inicio"><span class="brand-mark">M</span> MARY</a>
    <nav class="main-nav" id="mainNav">
      <a class="current" href="#inicio">⌂ Inicio</a>
      <a href="#herramientas">▦ Las dos herramientas</a>
      <a href="#biblioteca">▤ Biblioteca</a>
      <a href="#ayuda">ⓘ Ayuda</a>
    </nav>
    <button class="header-cta" onclick="irA('herramientas')">✦ Ver herramientas</button>
    <button class="mobile-toggle" onclick="document.getElementById('mainNav').classList.toggle('open')">☰</button>
  </div>
</header>

<main class="page">
  <section class="hero" id="inicio">
    <div class="hero-copy">
      <span class="pill">✦ TU ESTUDIO CREATIVO CON INTELIGENCIA ARTIFICIAL</span>
      <h1>MARY</h1>
      <div class="hero-subtitle">Tu espacio creativo con inteligencia artificial</div>
      <p class="hero-tagline">Busca, imagina, crea y <span>comparte.</span></p>
      <p class="hero-description">Un solo lugar para encontrar inspiración, crear imágenes, preparar videos y transformar tus ideas en contenido para tus redes.</p>
    </div>
    <div class="hero-art" aria-hidden="true">
      <div class="robot">
        <div class="robot-ear left"></div><div class="robot-ear right"></div>
        <div class="robot-head"><div class="robot-face"><i class="robot-eye"></i><i class="robot-eye"></i><i class="robot-smile"></i></div></div>
        <div class="robot-body"></div><div class="robot-badge">M</div>
        <div class="float-icon one">✦</div><div class="float-icon two">▣</div><div class="float-icon three">♫</div>
      </div>
    </div>
  </section>

  <section id="herramientas">
    <div class="section-heading">
      <div><div class="section-kicker">CENTRO DE CREACIÓN</div><h2>¿Qué quieres hacer hoy?</h2><p>Elige una de las dos herramientas principales.</p></div>
    </div>
    <div class="tools-grid">
      <button class="tool-card" onclick="abrirHerramienta('buscador')">
        <span class="tool-icon">🔎</span>
        <span class="tool-copy"><h3>Buscador IA</h3><p>Busca imágenes, pregunta a MARY y crea ideas, prompts, reflexiones, podcasts, historias y planes de video con audio.</p><span class="tool-points"><span>Buscar y crear imágenes</span><span>Videos y prompts</span><span>Textos para redes</span></span></span>
        <span class="tool-action">Explorar →</span>
      </button>
      <button class="tool-card" onclick="abrirHerramienta('clips')">
        <span class="tool-icon">✂️</span>
        <span class="tool-copy"><h3>Sacar clips</h3><p>Sube un video o pega un enlace directo autorizado para preparar varios cortes.</p><span class="tool-points"><span>Subir video</span><span>Enlace directo</span><span>Descargas MP4 individuales</span></span></span>
        <span class="tool-action">Sacar clips →</span>
      </button>
    </div>
  </section>

  <section class="benefits" aria-label="Ventajas de MARY">
    <div class="benefit"><b>ϟ</b><span>Rápido<br>y fácil</span></div>
    <div class="benefit"><b>♢</b><span>Herramientas<br>inteligentes</span></div>
    <div class="benefit"><b>⇩</b><span>Guarda tus<br>creaciones</span></div>
    <div class="benefit"><b>▣</b><span>Desde cualquier<br>dispositivo</span></div>
  </section>
  <div class="bottom-cta">Convierte tus ideas en contenido <span>increíble.</span> <button class="cta-button" onclick="abrirHerramienta('buscador')">🚀 ¡Comienza ahora!</button></div>

  <section class="work-area" id="area-herramienta">
    <section class="tool-panel" id="panel-clips">
      <div class="panel-head"><div><div class="section-kicker">HERRAMIENTA 02</div><h2>✂️ Sacar clips de video</h2><p>Selecciona cómo quieres proporcionar el video.</p></div><button class="close-panel" onclick="cerrarPanel()">✕ Cerrar</button></div>
      <div class="radio-row">
        <label class="radio-pill"><input type="radio" name="fuenteClip" value="subir" checked onchange="cambiarFuenteClip()"> ⬆️ Subir video</label>
        <label class="radio-pill"><input type="radio" name="fuenteClip" value="directo" onchange="cambiarFuenteClip()"> 🔗 Pegar enlace</label>
      </div>
      <div id="fuente-subir"><label class="field-label" for="archivoClip">Selecciona un video de tu dispositivo</label><input class="field" id="archivoClip" type="file" accept="video/mp4,video/quicktime,video/webm,video/x-matroska,video/x-msvideo"></div>
      <div id="fuente-directo" hidden><label class="field-label" for="enlaceClip">Pega el enlace directo del video</label><input class="field" id="enlaceClip" type="url" placeholder="https://servidor.com/video.mp4"><p class="field-help">Debe ser una URL directa a un archivo de video accesible. Los enlaces de páginas de YouTube, TikTok o Facebook no son archivos directos y no se descargan desde aquí.</p></div>
      <div class="panel-grid">
        <div><label class="field-label" for="modoCorteClip">¿Cómo quieres seleccionar los momentos?</label><select id="modoCorteClip" class="field" onchange="cambiarModoCorteClip()"><option value="automatico" selected>🤖 Automático: mejores momentos con IA</option><option value="manual">✂️ Manual: cortes consecutivos</option></select></div>
        <div id="estiloCorteContenedor"><label class="field-label" for="estiloCorteClip">¿Qué momentos debe buscar MARY?</label><select id="estiloCorteClip" class="field"><option value="mezcla" selected>🔥 Mezcla: graciosos y emocionantes</option><option value="gracioso">😂 Risas y momentos graciosos</option><option value="emocionante">❤️ Emocionantes y emotivos</option><option value="sorpresa">😲 Sorpresas y reacciones</option></select></div>
        <div id="inicioClipContenedor" hidden><label class="field-label" for="inicioClip">Comenzar en (segundos)</label><input class="field" id="inicioClip" type="number" min="0" step="0.5" value="0" placeholder="Ej. 45" oninput="actualizarMaximoClips()"></div>
        <div><label class="field-label" for="duracionClip">Duración de cada clip</label><select id="duracionClip" class="field" onchange="actualizarMaximoClips()"><option value="15">15 segundos</option><option value="30" selected>30 segundos</option><option value="60">60 segundos</option></select></div>
        <div><label class="field-label" for="cantidadClips">¿Cuántos clips quieres?</label><input class="field" id="cantidadClips" type="number" min="1" max="20" value="3" step="1"></div>
        <div><label class="field-label" for="formatoClip">Formato</label><select id="formatoClip" class="field"><option value="9:16">Vertical 9:16</option><option value="1:1">Cuadrado 1:1</option><option value="16:9">Horizontal 16:9</option></select></div>
        <div><label class="field-label" for="calidadClip">Calidad de exportación</label><select id="calidadClip" class="field"><option value="normal" selected>Normal · 720p</option><option value="alta">Alta calidad · 1080p</option></select><p class="field-help">La calidad final también depende de la resolución del video original.</p></div>
      </div>
      <div id="maximoClips" class="panel-note">La IA analizará el video y buscará momentos de humor, emoción, sorpresa y reacciones. MARY puede crear hasta 20 clips por proceso.</div>
      <video id="videoAnalisisClip" hidden muted playsinline preload="metadata"></video>
      <label class="field-label consent-row"><input id="clipAutorizado" type="checkbox"> Confirmo que soy dueño del video o tengo permiso para editarlo y publicarlo.</label>
      <div class="panel-note">El modo automático analiza el audio y las imágenes con Gemini para proponer momentos destacados. Los resultados son sugerencias: no garantizan viralidad. El modo manual conserva los cortes consecutivos. El análisis automático usa tu cuota de Gemini y envía temporalmente el video a Google para analizarlo; MARY solicita borrar el archivo al terminar.</div>
      <div id="clipAviso" class="output"></div>
      <button class="primary-button" id="botonClip" style="margin-top:16px" onclick="prepararClip()">✂️ Crear clips MP4</button>
    </section>

    <section class="tool-panel" id="panel-buscador">
      <div class="panel-head"><div><div class="section-kicker">HERRAMIENTA 01</div><h2>🔎 Buscador IA</h2><p>Entra a cualquiera de las herramientas creativas de MARY desde este mismo espacio.</p></div><button class="close-panel" onclick="cerrarPanel()">✕ Cerrar</button></div>

      <div class="subtool-grid">
    <button class="subtool-card" onclick="elegirTipo('Buscador IA')">
        <span>✨</span>
        <strong>Asistente de MARY</strong>
        <small>Escribe lo que necesitas y MARY te ayudará.</small>
    </button>
</div>
    <span>✨</span>
    <strong>Asistente de MARY</strong>
    <small>Escribe lo que necesitas y MARY te ayudará.</small>
</button>><span>🖼️</span><strong>Buscar imágenes</strong><small>Encuentra imágenes y revisa sus fuentes y licencias.</small></button>
      </div>

      <div class="panel-grid nested-workspace">
        <div class="nested-box" id="bloque-busqueda-imagenes">
          <h3>🔍 Buscar imágenes</h3>
          <p>Escribe qué imagen quieres encontrar. Se mostrarán resultados de Openverse con información de la fuente y licencia.</p>
          <label class="field-label" for="buscarImagen">¿Qué imagen deseas buscar?</label>
          <input class="field" id="buscarImagen" placeholder="Ejemplo: paisajes de montañas al amanecer">
          <button class="primary-button" style="margin-top:12px" onclick="buscarImagenes()">⌕ Buscar imágenes</button>
          <div class="panel-note">Revisa siempre la licencia, atribución y condiciones de cada imagen antes de publicarla.</div>
          <div id="resultadosImagenes" class="image-results"></div>
        </div>

        <div class="nested-box" id="bloque-generador">
          <h3>✦ Crear con MARY</h3>
          <p>Selecciona qué deseas crear y describe tu idea. MARY preparará el contenido con Gemini.</p>
          <label class="field-label" for="tipoContenido">¿Qué quieres crear?</label>
          <select id="tipoContenido" class="field">
            <option value="Buscador IA">Preguntar a MARY</option>
            <option value="Prompt para imagen">Crear imagen (prompt)</option>
            <option value="Guion de video con audio">Video con audio (guion y escenas)</option>
            <option value="Crear prompts">Crear o mejorar prompts</option>
            <option value="Reflexión motivacional">Reflexiones y mensajes</option>
            <option value="Podcast">Podcast</option>
            <option value="Historia o frutinovela">Historias y frutinovelas</option>
            <option value="Guion para TikTok y Facebook">Guiones para TikTok y Facebook</option>
          </select>
          <label class="field-label" for="temaContenido">Escribe tu tema o idea</label>
          <textarea id="temaContenido" placeholder="Ejemplo: una reflexión sobre nunca rendirse y seguir luchando por la familia..."></textarea>
          <div class="panel-grid">
            <div><label class="field-label" for="duracionContenido">Duración (para guiones)</label><select class="field" id="duracionContenido"><option>15 segundos</option><option selected>30 segundos</option><option>60 segundos</option></select></div>
            <div><label class="field-label" for="vozContenido">Voz sugerida</label><select class="field" id="vozContenido"><option>Voz cálida masculina</option><option>Voz cálida femenina</option><option>Voz motivadora</option><option>Sin narración</option></select></div>
          </div>
          <button class="primary-button" style="margin-top:16px" onclick="generarContenido()">✦ Generar con MARY</button>
          <div id="resultadoContenido" class="output"></div>
          <div class="panel-note">La creación de imágenes entrega el prompt; el video con audio entrega el guion y el plan de sonido. Para generar imágenes visuales o exportar un MP4 final hay que conectar servicios de imagen, voz y edición.</div>
        </div>
      </div>
    </section>
  </section>


</main>
<footer class="site-footer" id="ayuda">MARY · Tu espacio creativo con inteligencia artificial</footer>

<script>
function irA(id){document.getElementById(id).scrollIntoView({behavior:'smooth'});document.getElementById('mainNav').classList.remove('open')}
function abrirHerramienta(nombre){
  cerrarPanel();
  const panel=document.getElementById('panel-'+nombre);
  if(panel)panel.classList.add('visible');
  irA('area-herramienta');
}
function cerrarPanel(){document.querySelectorAll('.tool-panel').forEach(p=>p.classList.remove('visible'))}
let urlAnalisisClip=null;
let duracionDetectadaClip=null;
function cambiarFuenteClip(){
  const val=document.querySelector('input[name="fuenteClip"]:checked').value;
  document.getElementById('fuente-subir').hidden=val!=='subir';
  document.getElementById('fuente-directo').hidden=val!=='directo';
  analizarFuenteClip();
}
function analizarFuenteClip(){
  const video=document.getElementById('videoAnalisisClip');
  const fuente=document.querySelector('input[name="fuenteClip"]:checked').value;
  duracionDetectadaClip=null;
  document.getElementById('maximoClips').textContent='Calculando la duración del video...';
  if(urlAnalisisClip && urlAnalisisClip.startsWith('blob:')) URL.revokeObjectURL(urlAnalisisClip);
  urlAnalisisClip=null;
  if(fuente==='subir'){
    const archivo=document.getElementById('archivoClip').files[0];
    if(!archivo){document.getElementById('maximoClips').textContent='Selecciona un video para calcular cuántos clips completos puedes crear.';return;}
    urlAnalisisClip=URL.createObjectURL(archivo);
    video.src=urlAnalisisClip;
  }else{
    const url=document.getElementById('enlaceClip').value.trim();
    if(!url){document.getElementById('maximoClips').textContent='Pega un enlace directo de video para calcular la cantidad disponible.';return;}
    video.src=url;
  }
  video.load();
}
function cambiarModoCorteClip(){
  const automatico=document.getElementById('modoCorteClip').value==='automatico';
  document.getElementById('estiloCorteContenedor').hidden=!automatico;
  document.getElementById('inicioClipContenedor').hidden=automatico;
  const info=document.getElementById('maximoClips');
  if(automatico) info.textContent='MARY buscará los mejores momentos según el estilo elegido: humor, emoción, sorpresa o mezcla.';
  else actualizarMaximoClips();
}
function actualizarMaximoClips(){
  const info=document.getElementById('maximoClips');
  const cantidad=document.getElementById('cantidadClips');
  if(document.getElementById('modoCorteClip').value==='automatico'){
    info.textContent='La IA analizará el video y buscará hasta '+cantidad.value+' momentos de '+document.getElementById('duracionClip').value+' segundos. Máximo: 20 clips.';
    cantidad.max='20';return;
  }
  if(!Number.isFinite(duracionDetectadaClip)||duracionDetectadaClip<=0){
    info.textContent='La duración se comprobará en el servidor al procesar. Máximo: 20 clips por proceso.';
    cantidad.max='20';return;
  }
  const inicio=Math.max(0,Number(document.getElementById('inicioClip').value)||0);
  const clipDuracion=Number(document.getElementById('duracionClip').value)||30;
  const posibles=Math.max(0,Math.floor((duracionDetectadaClip-inicio)/clipDuracion));
  const limite=Math.min(20,posibles);
  cantidad.max=String(Math.max(1,limite));
  if(Number(cantidad.value)>limite && limite>0) cantidad.value=String(limite);
  info.textContent='Duración detectada: '+Math.floor(duracionDetectadaClip)+' s. Puedes crear hasta '+posibles+' clips completos de '+clipDuracion+' s (máximo 20 por proceso).';
  if(posibles===0) info.textContent='No hay espacio suficiente desde ese segundo para un clip completo de '+clipDuracion+' s.';
}
document.getElementById('videoAnalisisClip').addEventListener('loadedmetadata',()=>{duracionDetectadaClip=document.getElementById('videoAnalisisClip').duration;actualizarMaximoClips()});
document.getElementById('videoAnalisisClip').addEventListener('error',()=>{duracionDetectadaClip=null;document.getElementById('maximoClips').textContent='No se pudo leer la duración desde el navegador. El servidor comprobará el enlace al procesarlo.'});
document.getElementById('archivoClip').addEventListener('change',analizarFuenteClip);
document.getElementById('enlaceClip').addEventListener('change',analizarFuenteClip);
document.getElementById('duracionClip').addEventListener('change',actualizarMaximoClips);
document.getElementById('inicioClip').addEventListener('input',actualizarMaximoClips);
async function prepararClip(){
  const fuente=document.querySelector('input[name="fuenteClip"]:checked').value;
  const aviso=document.getElementById('clipAviso');
  const boton=document.getElementById('botonClip');
  const seleccion=document.getElementById('modoCorteClip').value;
  const automatico=seleccion==='automatico';
  aviso.classList.add('visible');
  if(!document.getElementById('clipAutorizado').checked){aviso.textContent='Confirma que eres dueño del video o tienes permiso para editarlo y publicarlo.';return;}
  const inicio=document.getElementById('inicioClip').value||'0';
  const duracion=document.getElementById('duracionClip').value;
  const cantidad=document.getElementById('cantidadClips').value||'1';
  const formato=document.getElementById('formatoClip').value;
  if(!Number.isInteger(Number(cantidad))||Number(cantidad)<1||Number(cantidad)>20){aviso.textContent='Elige entre 1 y 20 clips por proceso.';return;}
  const datos=new FormData();
  datos.append('modo',fuente);datos.append('seleccion',seleccion);datos.append('estilo',document.getElementById('estiloCorteClip').value);datos.append('inicio',inicio);datos.append('duracion',duracion);datos.append('cantidad',cantidad);datos.append('formato',formato);datos.append('calidad',document.getElementById('calidadClip').value);datos.append('autorizado','true');
  if(fuente==='subir'){
    const archivo=document.getElementById('archivoClip').files[0];
    if(!archivo){aviso.textContent='Selecciona un video antes de continuar.';return;}
    datos.append('video',archivo);
  }else{
    const url=document.getElementById('enlaceClip').value.trim();
    if(!url){aviso.textContent='Pega el enlace directo del video.';return;}
    datos.append('url',url);
  }
  boton.disabled=true;boton.textContent=automatico?'🤖 Analizando momentos...':'⏳ Creando clips...';
  aviso.textContent=automatico?'MARY está analizando el video para encontrar los momentos más graciosos, emocionantes o sorprendentes. Puede tardar unos minutos.':'MARY está creando los cortes consecutivos. No cierres esta página.';
  try{
    const respuesta=await fetch('/clips/subir',{method:'POST',body:datos});
    const resultado=await respuesta.json();
    if(!respuesta.ok)throw new Error(resultado.detail||resultado.error||'No se pudo procesar el video.');
    aviso.textContent='¡MARY creó '+resultado.clipsCreados+' clips de '+duracion+' segundos en calidad '+(resultado.calidad==='alta'?'alta (1080p)':'normal (720p)')+(resultado.seleccion==='automatico'?' seleccionados por IA.':'')+'. Descarga cada archivo MP4 por separado:';
    const lista=document.createElement('div');lista.className='clip-downloads';
    resultado.archivos.forEach((archivo,indice)=>{
      const item=document.createElement('div');item.className='clip-result-item';
      if(resultado.momentos && resultado.momentos[indice]){
        const momento=resultado.momentos[indice];
        const detalle=document.createElement('div');detalle.className='clip-result-detail';
        detalle.textContent='⭐ '+momento.categoria+' · '+momento.inicioTexto+' · potencial de retención estimado '+momento.potencial+'/100. '+momento.motivo;
        item.appendChild(detalle);
      }
      const enlace=document.createElement('a');
      enlace.href=archivo.downloadUrl;enlace.download=archivo.fileName;enlace.className='download-link';
      enlace.textContent='⬇ Descargar '+archivo.fileName;
      item.appendChild(enlace);lista.appendChild(item);
    });
    aviso.appendChild(lista);
  }catch(error){aviso.textContent='ERROR: '+error.message;}
  finally{boton.disabled=false;boton.textContent='✂️ Crear clips MP4';}
}
document.getElementById('duracionClip').addEventListener('change',actualizarMaximoClips);
document.getElementById('cantidadClips').addEventListener('input',actualizarMaximoClips);
cambiarModoCorteClip();
function mostrarBusquedaImagenes(){
 const bloque=document.getElementById('bloque-busqueda-imagenes');
 bloque.scrollIntoView({behavior:'smooth',block:'center'});
 document.getElementById('buscarImagen').focus();
}
function elegirTipo(tipo){
 const selector=document.getElementById('tipoContenido');
 selector.value=tipo;
 document.getElementById('bloque-generador').scrollIntoView({behavior:'smooth',block:'center'});
 document.getElementById('temaContenido').focus();
}
async function generarContenido(){
 const tipo=document.getElementById('tipoContenido').value;
 let tema=document.getElementById('temaContenido').value.trim();
 const duracion=document.getElementById('duracionContenido').value;
 const voz=document.getElementById('vozContenido').value;
 const out=document.getElementById('resultadoContenido');
 if(!tema){alert('Escribe tu tema o idea primero.');return;}
 if(tipo==='Guion de video con audio') tema+='\nDuración: '+duracion+'. Narración: '+voz+'. Formato vertical 9:16. Incluye escenas, texto en pantalla, narración exacta y sugerencias de música y efectos.';
 if(tipo==='Podcast'||tipo==='Reflexión motivacional'||tipo==='Historia o frutinovela'||tipo==='Guion para TikTok y Facebook') tema+='\nDuración aproximada solicitada: '+duracion+'.';
 out.classList.add('visible');out.textContent='MARY está creando tu contenido...';
 try{
  const texto=await postGenerar(tipo,tema);
  out.textContent=texto;
  agregarBotonCopiar(out,texto);
  guardarCreacion(tipo,texto);
 }catch(e){out.textContent='ERROR: '+e.message;}
}
async function postGenerar(tipo,tema){
 const r=await fetch('/generar',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({tipo,tema})});
 const d=await r.json();if(!r.ok)throw new Error(d.detail||d.error||'Error del servidor');return d.texto||'MARY no devolvió texto.';
}
async function buscarImagenes(){
 const q=document.getElementById('buscarImagen').value.trim();const cont=document.getElementById('resultadosImagenes');
 if(!q){alert('Escribe qué imagen deseas buscar.');return}
 cont.innerHTML='<p class="empty">Buscando imágenes...</p>';
 try{
  const r=await fetch('https://api.openverse.org/v1/images/?q='+encodeURIComponent(q)+'&page_size=12');
  if(!r.ok)throw new Error('El buscador de imágenes respondió con '+r.status);
  const data=await r.json();cont.innerHTML='';
  if(!data.results||!data.results.length){cont.innerHTML='<p class="empty">No encontramos imágenes para esa búsqueda.</p>';return}
  data.results.forEach(item=>{
   const card=document.createElement('article');card.className='image-card';
   const img=document.createElement('img');img.loading='lazy';img.src=item.thumbnail||item.url;img.alt=item.title||'Imagen encontrada';
   const meta=document.createElement('div');meta.className='image-meta';
   const title=document.createElement('strong');title.textContent=item.title||'Imagen sin título';
   const credit=document.createElement('div');credit.textContent='Autor: '+(item.creator||'No indicado')+' · Licencia: '+(item.license||'Revisar');
   const link=document.createElement('a');link.href=item.foreign_landing_url||item.url;link.target='_blank';link.rel='noopener noreferrer';link.textContent='Ver fuente y licencia';
   meta.append(title,credit,link);card.append(img,meta);cont.appendChild(card);
  });
 }catch(e){cont.innerHTML='';const p=document.createElement('p');p.className='empty';p.textContent='No se pudo completar la búsqueda: '+e.message;cont.appendChild(p)}
}
function agregarBotonCopiar(contenedor,texto){const b=document.createElement('button');b.className='secondary-button';b.textContent='Copiar resultado';b.onclick=()=>copiarTexto(texto);contenedor.appendChild(b)}
function cargarBiblioteca(){
 const lista=document.getElementById('lista-creaciones');if(!lista)return;let guardados=[];
 try{guardados=JSON.parse(localStorage.getItem('mary_creaciones')||localStorage.getItem('creadorIA')||'[]')}catch{guardados=[]}
 if(!guardados.length){lista.innerHTML='<p class="empty">Todavía no tienes creaciones guardadas.</p>';return}
 lista.innerHTML='';guardados.forEach((item,indice)=>{
  const div=document.createElement('div');div.className='saved-item';const titulo=document.createElement('strong');titulo.textContent=item.tipo||'Creación';
  const contenido=document.createElement('div');contenido.textContent=item.texto||'';
  const copiar=document.createElement('button');copiar.textContent='Copiar';copiar.onclick=()=>copiarTexto(item.texto||'');
  const borrar=document.createElement('button');borrar.textContent='Eliminar';borrar.onclick=()=>eliminarCreacion(indice);
  div.append(titulo,contenido,copiar,borrar);lista.appendChild(div);
 });
}
function guardarCreacion(tipo,texto){let guardados=[];try{guardados=JSON.parse(localStorage.getItem('mary_creaciones')||'[]')}catch{guardados=[]}guardados.unshift({tipo,texto});guardados=guardados.slice(0,20);localStorage.setItem('mary_creaciones',JSON.stringify(guardados));cargarBiblioteca()}
function eliminarCreacion(indice){let guardados=[];try{guardados=JSON.parse(localStorage.getItem('mary_creaciones')||'[]')}catch{guardados=[]}guardados.splice(indice,1);localStorage.setItem('mary_creaciones',JSON.stringify(guardados));cargarBiblioteca()}
async function copiarTexto(texto){try{await navigator.clipboard.writeText(texto);alert('Texto copiado.')}catch{alert('No se pudo copiar automáticamente. Selecciona el texto y cópialo.')}}

cargarBiblioteca();
</script>
</body>
</html>
""", "text/html; charset=utf-8"));

app.MapPost("/clips/subir", async (HttpRequest request, IWebHostEnvironment environment) =>
{
    const long maxBytes = 250L * 1024L * 1024L;
    string? localInputPath = null;
    string? sourceDownloadDirectory = null;
    string? workDirectory = null;
    string? persistentOutputDirectory = null;
    bool keepClipFiles = false;

    try
    {
        var form = await request.ReadFormAsync();
        string mode = string.IsNullOrWhiteSpace(form["modo"].ToString()) ? "subir" : form["modo"].ToString();
        bool authorized = bool.TryParse(form["autorizado"].ToString(), out bool confirmed) && confirmed;
        if (!authorized)
            return Results.BadRequest(new { error = "Confirma que eres dueño del video o tienes autorización para editarlo y publicarlo." });

        bool isRemote = mode == "directo";
        bool automaticSelection = form["seleccion"].ToString().Equals("automatico", StringComparison.OrdinalIgnoreCase);
        string style = form["estilo"].ToString().ToLowerInvariant();
        if (style is not ("mezcla" or "gracioso" or "emocionante" or "sorpresa")) style = "mezcla";
        string ffmpegInput;
        string extension;

        if (mode == "subir")
        {
            var video = form.Files.GetFile("video");
            if (video is null || video.Length == 0)
                return Results.BadRequest(new { error = "Selecciona un archivo de video." });
            if (video.Length > maxBytes)
                return Results.BadRequest(new { error = "El video supera el límite de 250 MB de esta versión." });

            extension = Path.GetExtension(video.FileName).ToLowerInvariant();
            string[] allowedExtensions = [".mp4", ".mov", ".m4v", ".webm", ".mkv", ".avi"];
            if (!allowedExtensions.Contains(extension))
                return Results.BadRequest(new { error = "Formato no admitido. Usa MP4, MOV, M4V, WEBM, MKV o AVI." });

            string uploadDirectory = Path.Combine(Path.GetTempPath(), "mary-video-work");
            Directory.CreateDirectory(uploadDirectory);
            localInputPath = Path.Combine(uploadDirectory, Guid.NewGuid().ToString("N") + extension);
            await using (var stream = File.Create(localInputPath))
                await video.CopyToAsync(stream);
            ffmpegInput = localInputPath;
        }
        else if (isRemote)
        {
            string rawUrl = form["url"].ToString().Trim();

            if (IsSupportedPlatformVideoUrl(rawUrl))
            {
                sourceDownloadDirectory = Path.Combine(Path.GetTempPath(), "mary-video-work", Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(sourceDownloadDirectory);
                var platformDownload = await DownloadPlatformVideoAsync(rawUrl, sourceDownloadDirectory, maxBytes);
                if (platformDownload.Error is not null)
                    return Results.BadRequest(new { error = platformDownload.Error });

                localInputPath = platformDownload.Path!;
                extension = Path.GetExtension(localInputPath).ToLowerInvariant();
                ffmpegInput = localInputPath;
                isRemote = false;
            }
            else
            {
                string? validationError = await ValidateRemoteMediaUrlAsync(rawUrl, "directo");
                if (validationError is not null)
                    return Results.BadRequest(new { error = validationError });

                var mediaUri = new Uri(rawUrl);
                extension = Path.GetExtension(mediaUri.AbsolutePath).ToLowerInvariant();
                if (automaticSelection)
                {
                    if (extension == ".m3u8")
                        return Results.BadRequest(new { error = "El anÃ¡lisis automÃ¡tico necesita un archivo MP4, MOV, WEBM, MKV o AVI. Para HLS usa el modo manual o sube una grabaciÃ³n autorizada." });
                    string remoteWork = Path.Combine(Path.GetTempPath(), "mary-video-work");
                    Directory.CreateDirectory(remoteWork);
                    localInputPath = Path.Combine(remoteWork, Guid.NewGuid().ToString("N") + (string.IsNullOrWhiteSpace(extension) ? ".mp4" : extension));
                    string? downloadError = await DownloadDirectVideoAsync(mediaUri.AbsoluteUri, localInputPath, maxBytes);
                    if (downloadError is not null)
                        return Results.BadRequest(new { error = downloadError });
                    ffmpegInput = localInputPath;
                    isRemote = false;
                }
                else
                {
                    ffmpegInput = mediaUri.AbsoluteUri;
                }
            }
        }        else
        {
            return Results.BadRequest(new { error = "Selecciona subir un video o pegar un enlace directo." });
        }

        if (!double.TryParse(form["inicio"].ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double startSeconds) ||
            startSeconds < 0 || startSeconds > 86400)
            return Results.BadRequest(new { error = "El segundo inicial no es válido." });

        if (!int.TryParse(form["duracion"].ToString(), out int durationSeconds) || !new[] { 15, 30, 60 }.Contains(durationSeconds))
            return Results.BadRequest(new { error = "La duración debe ser 15, 30 o 60 segundos." });

        if (!int.TryParse(form["cantidad"].ToString(), out int requestedClips) || requestedClips < 1 || requestedClips > 20)
            return Results.BadRequest(new { error = "Puedes crear entre 1 y 20 clips por proceso." });

        double? mediaDuration = await GetMediaDurationAsync(ffmpegInput, isRemote);
        if (mediaDuration is null || mediaDuration <= 0)
            return Results.BadRequest(new { error = "MARY no pudo detectar la duración. Comprueba que el enlace sea un archivo de video directo y accesible, o prueba con un MP4 descargado." });

        int possibleClips = (int)Math.Floor((mediaDuration.Value - startSeconds + 0.0001) / durationSeconds);
        if (possibleClips < 1)
            return Results.BadRequest(new { error = $"El video dura {Math.Floor(mediaDuration.Value)} segundos. Desde el segundo {startSeconds}, no cabe un clip completo de {durationSeconds} segundos." });
        if (!automaticSelection && requestedClips > possibleClips)
            return Results.BadRequest(new { error = $"Este video permite {possibleClips} clips completos de {durationSeconds} segundos desde el segundo {startSeconds}. Reduce la cantidad solicitada." });

        var selectedMoments = new List<ClipMoment>();
        var clipStarts = new List<double>();
        if (automaticSelection)
        {
            if (localInputPath is null || !File.Exists(localInputPath))
                return Results.BadRequest(new { error = "Para buscar momentos automáticamente, MARY necesita poder leer el archivo de video. Sube el archivo o utiliza un enlace directo descargable." });
            string? groqKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");
            string? openRouterKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");
            if (string.IsNullOrWhiteSpace(groqKey))
                return Results.Problem(detail: "Falta configurar GROQ_API_KEY. Groq se utiliza para transcribir el audio del video.", statusCode: 500);
            var analysis = await DetectHighlightMomentsAsync(localInputPath, groqKey, openRouterKey, mediaDuration.Value, durationSeconds, requestedClips, style);if (analysis.Error is not null)
                return Results.Problem(detail: analysis.Error, statusCode: 502);
            selectedMoments = analysis.Moments;
            if (selectedMoments.Count == 0)
                return Results.Problem(detail: "La IA no encontró momentos claros para clips de esa duración. Prueba con 15 segundos o selecciona el modo manual.", statusCode: 422);
            clipStarts = selectedMoments.Select(m => m.Start).ToList();
            requestedClips = clipStarts.Count;
        }
        else
        {
            for (int i = 0; i < requestedClips; i++) clipStarts.Add(startSeconds + (i * durationSeconds));
        }

        string aspect = form["formato"].ToString();
        string quality = form["calidad"].ToString().Equals("alta", StringComparison.OrdinalIgnoreCase) ? "alta" : "normal";
        int targetWidth = (aspect, quality) switch
        {
            ("9:16", "alta") => 1080,
            ("9:16", _) => 720,
            ("1:1", "alta") => 1080,
            ("1:1", _) => 720,
            ("16:9", "alta") => 1920,
            ("16:9", _) => 1280,
            _ => 720
        };
        int targetHeight = (aspect, quality) switch
        {
            ("9:16", "alta") => 1920,
            ("9:16", _) => 1280,
            ("1:1", "alta") => 1080,
            ("1:1", _) => 720,
            ("16:9", "alta") => 1080,
            ("16:9", _) => 720,
            _ => 1280
        };
        string videoFilter = $"scale={targetWidth}:{targetHeight}:force_original_aspect_ratio=increase,crop={targetWidth}:{targetHeight}";
        string videoCrf = quality == "alta" ? "19" : "25";
        string videoPreset = quality == "alta" ? "medium" : "veryfast";
        string audioBitrate = quality == "alta" ? "192k" : "128k";

        string id = Guid.NewGuid().ToString("N");
        string outputRoot = Path.Combine(environment.ContentRootPath, "App_Data", "clips");
        persistentOutputDirectory = Path.Combine(outputRoot, id);
        Directory.CreateDirectory(persistentOutputDirectory);
        workDirectory = Path.Combine(Path.GetTempPath(), "mary-clips", id);
        Directory.CreateDirectory(workDirectory);

        var clipPaths = new List<string>();
        for (int index = 0; index < requestedClips; index++)
        {
            string clipPath = Path.Combine(workDirectory, $"MARY-clip-{index + 1:00}.mp4");
            double clipStart = clipStarts[index];
            var startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var arguments = new List<string> { "-hide_banner", "-loglevel", "error", "-y" };
            if (isRemote)
                arguments.AddRange(["-protocol_whitelist", "http,https,tcp,tls,crypto,data", "-rw_timeout", "20000000"]);
            arguments.AddRange([
                "-ss", clipStart.ToString("0.###", CultureInfo.InvariantCulture),
                "-i", ffmpegInput,
                "-t", durationSeconds.ToString(CultureInfo.InvariantCulture),
                "-map", "0:v:0", "-map", "0:a?",
                "-vf", videoFilter,
                "-c:v", "libx264", "-preset", videoPreset, "-crf", videoCrf,
                "-pix_fmt", "yuv420p", "-c:a", "aac", "-b:a", audioBitrate,
                "-movflags", "+faststart", clipPath
            ]);
            foreach (string argument in arguments)
                startInfo.ArgumentList.Add(argument);

            using var process = new Process { StartInfo = startInfo };
            try
            {
                if (!process.Start())
                    return Results.Problem(detail: "No se pudo iniciar FFmpeg.", statusCode: 500);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return Results.Problem(detail: "No se encontró FFmpeg. Instálalo y agrégalo al PATH; en Render debe instalarse en el Dockerfile.", statusCode: 503);
            }

            Task<string> errorTask = process.StandardError.ReadToEndAsync();
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(300));
            try
            {
                await process.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                try { process.Kill(entireProcessTree: true); } catch { }
                return Results.Problem(detail: "El procesamiento superó los 5 minutos. Prueba con un video más corto o crea menos clips.", statusCode: 504);
            }

            string ffmpegError = await errorTask;
            if (process.ExitCode != 0 || !File.Exists(clipPath) || new FileInfo(clipPath).Length == 0)
            {
                Console.WriteLine($"FFmpeg no pudo crear el clip {index + 1}. Código: {process.ExitCode}. {ffmpegError}");
                return Results.Problem(detail: isRemote
                    ? "No se pudo leer el enlace. Usa una URL directa a un archivo compatible (por ejemplo MP4 o MOV) y comprueba que tengas permiso. Las páginas normales de YouTube, TikTok y Facebook no son enlaces directos de archivo."
                    : "No se pudo cortar el video. Comprueba que el archivo esté en buen estado.", statusCode: 422);
            }
            clipPaths.Add(clipPath);
        }

        var downloadableClips = new List<object>();
        for (int index = 0; index < clipPaths.Count; index++)
        {
            string fileName = $"MARY-clip-{index + 1:00}-{quality}.mp4";
            string destinationPath = Path.Combine(persistentOutputDirectory, fileName);
            File.Copy(clipPaths[index], destinationPath, overwrite: true);
            downloadableClips.Add(new
            {
                numero = index + 1,
                fileName,
                downloadUrl = $"/clips/descargar/{id}/{index + 1}"
            });
        }

        keepClipFiles = true;
        return Results.Ok(new
        {
            clipsCreados = requestedClips,
            clips = requestedClips,
            calidad = quality,
            resolucion = $"{targetWidth}x{targetHeight}",
            duracionCadaClip = durationSeconds,
            duracionVideo = Math.Round(mediaDuration.Value, 1),
            clipsDisponibles = possibleClips,
            seleccion = automaticSelection ? "automatico" : "manual",
            momentos = selectedMoments.Select(m => new
            {
                inicio = m.Start,
                fin = m.End,
                inicioTexto = TimeSpan.FromSeconds(m.Start).ToString(@"hh\:mm\:ss"),
                categoria = m.Category,
                motivo = m.Reason,
                potencial = m.Potential
            }).ToList(),
            archivos = downloadableClips
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR AL PROCESAR CLIPS: " + ex.Message);
        return Results.Problem(detail: "No se pudo procesar el video: " + ex.Message, statusCode: 500);
    }
    finally
    {
        if (localInputPath is not null && File.Exists(localInputPath))
        {
            try { File.Delete(localInputPath); } catch { }
        }
        if (sourceDownloadDirectory is not null && Directory.Exists(sourceDownloadDirectory))
        {
            try { Directory.Delete(sourceDownloadDirectory, recursive: true); } catch { }
        }        if (workDirectory is not null && Directory.Exists(workDirectory))
        {
            try { Directory.Delete(workDirectory, recursive: true); } catch { }
        }
        if (!keepClipFiles && persistentOutputDirectory is not null && Directory.Exists(persistentOutputDirectory))
        {
            try { Directory.Delete(persistentOutputDirectory, recursive: true); } catch { }
        }
    }
});

app.MapGet("/clips/descargar/{id}/{clipNumber:int}", (string id, int clipNumber, IWebHostEnvironment environment) =>
{
    if (!Guid.TryParseExact(id, "N", out _) || clipNumber < 1 || clipNumber > 20)
        return Results.BadRequest(new { error = "Identificador o número de clip no válido." });

    string fileName = $"MARY-clip-{clipNumber:00}-";
    string directory = Path.Combine(environment.ContentRootPath, "App_Data", "clips", id);
    if (!Directory.Exists(directory))
        return Results.NotFound(new { error = "El clip ya no está disponible. Procésalo nuevamente." });

    string? path = Directory.EnumerateFiles(directory, fileName + "*.mp4", SearchOption.TopDirectoryOnly).FirstOrDefault();
    if (path is null || !File.Exists(path))
        return Results.NotFound(new { error = "El clip solicitado no está disponible. Procésalo nuevamente." });

    return Results.File(path, "video/mp4", Path.GetFileName(path), enableRangeProcessing: true);
});

static async Task<double?> GetMediaDurationAsync(string input, bool isRemote)
{
    var startInfo = new ProcessStartInfo
    {
        FileName = "ffprobe",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };
    startInfo.ArgumentList.Add("-v"); startInfo.ArgumentList.Add("error");
    if (isRemote)
    {
        startInfo.ArgumentList.Add("-protocol_whitelist");
        startInfo.ArgumentList.Add("http,https,tcp,tls,crypto,data");
        startInfo.ArgumentList.Add("-rw_timeout"); startInfo.ArgumentList.Add("20000000");
    }
    startInfo.ArgumentList.Add("-show_entries"); startInfo.ArgumentList.Add("format=duration");
    startInfo.ArgumentList.Add("-of"); startInfo.ArgumentList.Add("default=noprint_wrappers=1:nokey=1");
    startInfo.ArgumentList.Add(input);

    using var process = new Process { StartInfo = startInfo };
    try
    {
        if (!process.Start()) return null;
        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
        Task<string> errorTask = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(45));
        await process.WaitForExitAsync(timeout.Token);
        string output = await outputTask;
        _ = await errorTask;
        if (process.ExitCode != 0 || !double.TryParse(output.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double duration))
            return null;
        return duration > 0 && double.IsFinite(duration) ? duration : null;
    }
    catch (Exception ex)
    {
        try { if (!process.HasExited) process.Kill(entireProcessTree: true); } catch { }
        Console.WriteLine("FFprobe no pudo obtener la duración: " + ex.Message);
        return null;
    }
}

static async Task<string?> DownloadDirectVideoAsync(string initialUrl, string destinationPath, long maxBytes)
{
    using var handler = new HttpClientHandler { AllowAutoRedirect = false };
    using var client = new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(3) };
    Uri current = new(initialUrl);

    for (int redirect = 0; redirect <= 4; redirect++)
    {
        string? validation = await ValidatePublicHttpUriAsync(current);
        if (validation is not null) return validation;

        using var request = new HttpRequestMessage(HttpMethod.Get, current);
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if ((int)response.StatusCode is >= 300 and < 400)
        {
            if (redirect == 4 || response.Headers.Location is null)
                return "El enlace tiene demasiadas redirecciones. Pega la URL final directa del archivo de video.";
            current = response.Headers.Location.IsAbsoluteUri ? response.Headers.Location : new Uri(current, response.Headers.Location);
            continue;
        }
        if (!response.IsSuccessStatusCode)
            return $"No se pudo descargar el enlace para analizarlo (HTTP {(int)response.StatusCode}). Comprueba que sea un archivo directo y accesible.";

        long? length = response.Content.Headers.ContentLength;
        if (length.HasValue && length.Value > maxBytes)
            return "El video del enlace supera el límite de 250 MB para el análisis automático.";
        string contentType = response.Content.Headers.ContentType?.MediaType ?? "";
        if (!contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase) &&
            !contentType.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase) &&
            !contentType.Equals("binary/octet-stream", StringComparison.OrdinalIgnoreCase))
            return "El enlace no devolvió un archivo de video. Usa una URL directa al MP4 o MOV, no la página de una publicación.";

        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
        await using var input = await response.Content.ReadAsStreamAsync();
        await using var output = File.Create(destinationPath);
        byte[] buffer = new byte[81920];
        long total = 0;
        while (true)
        {
            int read = await input.ReadAsync(buffer.AsMemory(0, buffer.Length));
            if (read == 0) break;
            total += read;
            if (total > maxBytes)
            {
                output.Close();
                try { File.Delete(destinationPath); } catch { }
                return "El video del enlace supera el límite de 250 MB para el análisis automático.";
            }
            await output.WriteAsync(buffer.AsMemory(0, read));
        }
        if (total == 0) return "El enlace devolvió un archivo vacío.";
        return null;
    }
    return "No se pudo acceder al enlace directo.";
}

static bool IsSupportedPlatformVideoUrl(string rawUrl)
{
    if (!Uri.TryCreate(rawUrl, UriKind.Absolute, out var uri)) return false;
    if (uri.Scheme is not ("http" or "https") || uri.UserInfo.Length > 0 || !uri.IsDefaultPort) return false;

    string host = uri.IdnHost.ToLowerInvariant();
    static bool Matches(string hostName, string domain) =>
        hostName.Equals(domain, StringComparison.OrdinalIgnoreCase) ||
        hostName.EndsWith("." + domain, StringComparison.OrdinalIgnoreCase);

    return Matches(host, "youtube.com") ||
           Matches(host, "youtu.be") ||
           Matches(host, "youtube-nocookie.com") ||
           Matches(host, "tiktok.com") ||
           Matches(host, "facebook.com") ||
           Matches(host, "fb.watch");
}

static async Task<(string? Path, string? Error)> DownloadPlatformVideoAsync(string url, string destinationDirectory, long maxBytes)
{
    Directory.CreateDirectory(destinationDirectory);
    var startInfo = new ProcessStartInfo
    {
        FileName = "yt-dlp",
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    };

    startInfo.ArgumentList.Add("--no-playlist");
    startInfo.ArgumentList.Add("--no-progress");
    startInfo.ArgumentList.Add("--no-warnings");
    startInfo.ArgumentList.Add("--no-write-info-json");
    startInfo.ArgumentList.Add("--no-write-thumbnail");
    startInfo.ArgumentList.Add("--no-write-subs");
    startInfo.ArgumentList.Add("--no-write-auto-subs");
    startInfo.ArgumentList.Add("--max-filesize");
    startInfo.ArgumentList.Add("250M");
    startInfo.ArgumentList.Add("--format");
    startInfo.ArgumentList.Add("bv*+ba/b");
    startInfo.ArgumentList.Add("--merge-output-format");
    startInfo.ArgumentList.Add("mp4");
    startInfo.ArgumentList.Add("--remux-video");
    startInfo.ArgumentList.Add("mp4");
    startInfo.ArgumentList.Add("--output");
    startInfo.ArgumentList.Add(Path.Combine(destinationDirectory, "mary-source.%(ext)s"));
    startInfo.ArgumentList.Add(url);

    using var process = new Process { StartInfo = startInfo };
    try
    {
        if (!process.Start())
            return (null, "MARY no pudo iniciar yt-dlp. Comprueba que estÃ© instalado y disponible en PATH.");

        Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
        Task<string> errorTask = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(8));
        await process.WaitForExitAsync(timeout.Token);
        _ = await outputTask;
        string error = await errorTask;

        if (process.ExitCode != 0)
        {
            Console.WriteLine("yt-dlp: " + error);
            return (null, "No se pudo obtener el video. Comprueba que sea pÃºblico, que el enlace sea correcto y que la plataforma permita acceder a Ã©l.");
        }

        string? downloaded = Directory.GetFiles(destinationDirectory, "mary-source.*")
            .Where(path => !path.EndsWith(".part", StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(File.GetLastWriteTimeUtc)
            .FirstOrDefault();

        if (downloaded is null || !File.Exists(downloaded))
            return (null, "yt-dlp terminÃ³, pero no se encontrÃ³ el archivo de video descargado.");

        var info = new FileInfo(downloaded);
        if (info.Length <= 0)
            return (null, "La plataforma devolviÃ³ un archivo de video vacÃ­o.");
        if (info.Length > maxBytes)
            return (null, "El video supera el lÃ­mite de 250 MB permitido por MARY.");

        string extension = Path.GetExtension(downloaded).ToLowerInvariant();
        if (extension is not (".mp4" or ".mov" or ".m4v" or ".webm" or ".mkv" or ".avi"))
            return (null, "La plataforma entregÃ³ un formato de video no compatible.");

        return (downloaded, null);
    }
    catch (OperationCanceledException)
    {
        try { if (!process.HasExited) process.Kill(entireProcessTree: true); } catch { }
        return (null, "La descarga tardÃ³ demasiado y MARY la cancelÃ³. IntÃ©ntalo con un video mÃ¡s corto.");
    }
    catch (Exception ex)
    {
        try { if (!process.HasExited) process.Kill(entireProcessTree: true); } catch { }
        Console.WriteLine("Error de yt-dlp: " + ex.Message);
        return (null, "MARY no pudo ejecutar yt-dlp. Verifica que estÃ© instalado y disponible en PATH.");
    }
}
static async Task<string?> ValidatePublicHttpUriAsync(Uri uri)
{
    if (uri.Scheme is not ("http" or "https") || uri.UserInfo.Length > 0)
        return "Para el análisis automático usa enlaces HTTP o HTTPS públicos, sin usuario ni contraseña.";
    if ((uri.Scheme == "http" && uri.Port != 80) || (uri.Scheme == "https" && uri.Port != 443))
        return "El enlace usa un puerto no permitido por seguridad.";
    if (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || uri.Host.EndsWith(".local", StringComparison.OrdinalIgnoreCase))
        return "No se permiten direcciones locales o privadas.";
    try
    {
        IPAddress[] addresses = IPAddress.TryParse(uri.DnsSafeHost, out var literal) ? [literal] : await Dns.GetHostAddressesAsync(uri.DnsSafeHost);
        if (addresses.Length == 0 || addresses.Any(address => !IsPublicInternetAddress(address)))
            return "El servidor del enlace no parece tener una dirección pública.";
    }
    catch
    {
        return "MARY no pudo comprobar el servidor del enlace.";
    }
    return null;
}

static async Task<(List<ClipMoment> Moments, string? Error)> DetectHighlightMomentsAsync(
    string videoPath, string groqKey, string? openRouterKey, double videoDuration, int clipDuration, int desiredCount, string style)
{
    const int chunkSeconds = 600;
    string audioDirectory = Path.Combine(Path.GetTempPath(), "mary-audio-" + Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(audioDirectory);

    try
    {
        var fileInfo = new FileInfo(videoPath);
        if (!fileInfo.Exists || fileInfo.Length == 0)
            return ([], "No se encontró el video para analizar.");
        if (fileInfo.Length > 250L * 1024L * 1024L)
            return ([], "El video supera el límite de 250 MB para el análisis automático.");

        // Divide el audio en bloques de 10 minutos, mono y comprimidos.
        // Así cada archivo queda muy por debajo del límite de carga de Groq.
        string audioPattern = Path.Combine(audioDirectory, "audio-%03d.mp3");
        var ffmpegStart = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (string argument in new[]
        {
            "-hide_banner", "-loglevel", "error", "-y",
            "-i", videoPath, "-vn", "-ac", "1", "-ar", "16000",
            "-b:a", "32k", "-f", "segment", "-segment_time",
            chunkSeconds.ToString(CultureInfo.InvariantCulture),
            "-reset_timestamps", "1", audioPattern
        })
            ffmpegStart.ArgumentList.Add(argument);

        using (var ffmpeg = new Process { StartInfo = ffmpegStart })
        {
            if (!ffmpeg.Start())
                return ([], "No se pudo iniciar FFmpeg para extraer el audio.");
            Task<string> ffmpegErrorTask = ffmpeg.StandardError.ReadToEndAsync();
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(8));
            await ffmpeg.WaitForExitAsync(timeout.Token);
            string ffmpegError = await ffmpegErrorTask;
            if (ffmpeg.ExitCode != 0)
                return ([], "FFmpeg no pudo extraer el audio. Comprueba que el video tenga una pista de sonido. " + ffmpegError);
        }

        string[] audioFiles = Directory.GetFiles(audioDirectory, "audio-*.mp3")
            .OrderBy(path => Path.GetFileName(path), StringComparer.Ordinal)
            .ToArray();
        if (audioFiles.Length == 0)
            return ([], "No se encontró una pista de audio en el video.");

        using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        var transcript = new StringBuilder();

        // Transcribir cada bloque con marcas de tiempo y reconstruir el tiempo del video original.
        for (int chunkIndex = 0; chunkIndex < audioFiles.Length; chunkIndex++)
        {
            string audioPath = audioFiles[chunkIndex];
            using var multipart = new MultipartFormDataContent();
            multipart.Add(new StringContent("whisper-large-v3-turbo"), "model");
            multipart.Add(new StringContent("verbose_json"), "response_format");
            multipart.Add(new StringContent("es"), "language");
            multipart.Add(new StringContent("segment"), "timestamp_granularities[]");

            var audioStream = File.OpenRead(audioPath);
            var audioContent = new StreamContent(audioStream);
            audioContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("audio/mpeg");
            multipart.Add(audioContent, "file", Path.GetFileName(audioPath));

            using var transcriptionRequest = new HttpRequestMessage(
                HttpMethod.Post, "https://api.groq.com/openai/v1/audio/transcriptions");
            transcriptionRequest.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", groqKey);
            transcriptionRequest.Content = multipart;

            using var transcriptionResponse = await client.SendAsync(transcriptionRequest);
            string transcriptionBody = await transcriptionResponse.Content.ReadAsStringAsync();
            if (!transcriptionResponse.IsSuccessStatusCode)
                return ([], $"Groq no pudo transcribir el bloque {chunkIndex + 1} (HTTP {(int)transcriptionResponse.StatusCode}): {transcriptionBody}");

            using var transcriptionJson = JsonDocument.Parse(transcriptionBody);
            if (!transcriptionJson.RootElement.TryGetProperty("segments", out var segments) ||
                segments.ValueKind != JsonValueKind.Array)
                return ([], "Groq recibió el audio, pero no devolvió marcas de tiempo. Inténtalo de nuevo.");

            double offset = chunkIndex * chunkSeconds;
            foreach (var segment in segments.EnumerateArray())
            {
                if (!segment.TryGetProperty("start", out var startElement) ||
                    !segment.TryGetProperty("end", out var endElement) ||
                    !startElement.TryGetDouble(out double localStart) ||
                    !endElement.TryGetDouble(out double localEnd))
                    continue;

                string spokenText = segment.TryGetProperty("text", out var textElement)
                    ? textElement.GetString() ?? ""
                    : "";
                if (string.IsNullOrWhiteSpace(spokenText)) continue;

                double absoluteStart = offset + localStart;
                double absoluteEnd = offset + localEnd;
                transcript.Append('[')
                    .Append(absoluteStart.ToString("0.0", CultureInfo.InvariantCulture))
                    .Append('-')
                    .Append(absoluteEnd.ToString("0.0", CultureInfo.InvariantCulture))
                    .Append("] ")
                    .AppendLine(spokenText.Trim());
            }
        }

        string transcriptText = transcript.ToString();
        if (string.IsNullOrWhiteSpace(transcriptText))
            return ([], "Groq no detectó palabras claras en el audio. Puedes probar el modo manual.");

        // Evitar enviar un texto excesivamente largo al modelo de selección.
        if (transcriptText.Length > 60000)
            transcriptText = transcriptText[..60000];

        string styleInstruction = style switch
        {
            "gracioso" => "Prioriza bromas, chistes, carcajadas descritas en el diálogo, remates cómicos y conversaciones divertidas.",
            "emocionante" => "Prioriza frases emotivas, inspiradoras, tensas o emocionantes y conversaciones memorables.",
            "sorpresa" => "Prioriza revelaciones, giros inesperados expresados en palabras y reacciones que se perciban en el diálogo.",
            _ => "Busca una mezcla de humor, emoción, sorpresa, energía y frases memorables."
        };

        string prompt = $$"""
Eres MARY, especialista en seleccionar fragmentos atractivos para videos cortos.
Analiza esta transcripción en español, que incluye los segundos del video original.
Selecciona hasta {{desiredCount}} momentos, cada uno de exactamente {{clipDuration}} segundos.
{{styleInstruction}}
Elige fragmentos que enganchen rápido y tengan una recompensa clara: una broma, un remate, una frase fuerte, una conversación interesante o una revelación.
Evita silencios, esperas, introducciones largas, repeticiones y fragmentos que no se entiendan fuera de contexto.
No inventes diálogos, sucesos ni marcas de tiempo. Basa cada selección en las marcas de tiempo de la transcripción.
El video dura {{videoDuration.ToString("0.0", CultureInfo.InvariantCulture)}} segundos.
Devuelve SOLO JSON válido, sin Markdown ni explicaciones, con esta estructura:
{"momentos":[{"inicio":12.5,"fin":{{clipDuration}},"categoria":"gracioso","motivo":"Explicación breve basada en lo que se dice","potencial":88}]}
"inicio" y "fin" son segundos del video original; "fin" debe ser igual a "inicio" más {{clipDuration}}.
"potencial" es una valoración orientativa de 1 a 100, no una predicción de viralidad.
No superpongas los momentos. Si no hay suficientes momentos buenos, devuelve menos en vez de inventarlos.

TRANSCRIPCIÓN:
{{transcriptText}}
""";

        async Task<(string? Text, string Error)> AskModelAsync(string url, string key, string model)
        {
            try
            {
                var requestBody = new
                {
                    model,
                    messages = new[]
                    {
                        new { role = "system", content = "Eres MARY. Responde únicamente con el JSON solicitado y no inventes marcas de tiempo." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.2
                };

                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
                request.Content = new StringContent(
                    JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                using var response = await client.SendAsync(request);
                string body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return (null, $"HTTP {(int)response.StatusCode}: {body}");

                using var json = JsonDocument.Parse(body);
                if (!json.RootElement.TryGetProperty("choices", out var choices) ||
                    choices.GetArrayLength() == 0 ||
                    !choices[0].TryGetProperty("message", out var message) ||
                    !message.TryGetProperty("content", out var content))
                    return (null, "El modelo no devolvió contenido válido.");

                string? answer = content.ValueKind == JsonValueKind.String
                    ? content.GetString()
                    : content.ToString();
                return !string.IsNullOrWhiteSpace(answer)
                    ? (answer, "")
                    : (null, "El modelo devolvió una respuesta vacía.");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        string? modelAnswer = null;
        string providerError = "";

        var groqAnalysis = await AskModelAsync(
            "https://api.groq.com/openai/v1/chat/completions",
            groqKey,
            "openai/gpt-oss-20b");
        modelAnswer = groqAnalysis.Text;
        providerError = groqAnalysis.Error;

        if (modelAnswer is null && !string.IsNullOrWhiteSpace(openRouterKey))
        {
            Console.WriteLine("MARY: Groq no pudo seleccionar los clips; intentando OpenRouter gratuito.");
            var openRouterAnalysis = await AskModelAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                openRouterKey,
                "openrouter/free");
            modelAnswer = openRouterAnalysis.Text;
            if (modelAnswer is null)
                providerError += " | OpenRouter: " + openRouterAnalysis.Error;
        }

        if (modelAnswer is null)
            return ([], "No se pudieron seleccionar los momentos con Groq ni OpenRouter. " + providerError);

        string jsonText = modelAnswer.Trim();
        int firstBrace = jsonText.IndexOf('{');
        int lastBrace = jsonText.LastIndexOf('}');
        if (firstBrace < 0 || lastBrace <= firstBrace)
            return ([], "La IA no devolvió el JSON de momentos esperado. Inténtalo de nuevo o usa el modo manual.");

        using var momentsJson = JsonDocument.Parse(jsonText.Substring(firstBrace, lastBrace - firstBrace + 1));
        if (!momentsJson.RootElement.TryGetProperty("momentos", out var momentArray) ||
            momentArray.ValueKind != JsonValueKind.Array)
            return ([], "La IA no devolvió una lista válida de momentos destacados.");

        var candidatesList = new List<ClipMoment>();
        foreach (var item in momentArray.EnumerateArray())
        {
            if (!item.TryGetProperty("inicio", out var startValue) ||
                !startValue.TryGetDouble(out double start))
                continue;

            if (!double.IsFinite(start) || start < 0 || start + clipDuration > videoDuration)
                continue;

            string category = item.TryGetProperty("categoria", out var categoryValue)
                ? categoryValue.GetString() ?? "destacado"
                : "destacado";
            string reason = item.TryGetProperty("motivo", out var reasonValue)
                ? reasonValue.GetString() ?? "Momento destacado"
                : "Momento destacado";
            int score = item.TryGetProperty("potencial", out var scoreValue) &&
                        scoreValue.TryGetInt32(out int parsedScore)
                ? Math.Clamp(parsedScore, 1, 100)
                : 70;

            candidatesList.Add(new ClipMoment(start, start + clipDuration, category, reason, score));
        }

        var accepted = new List<ClipMoment>();
        foreach (var candidate in candidatesList.OrderByDescending(m => m.Potential))
        {
            if (accepted.Any(existing => Math.Abs(existing.Start - candidate.Start) < clipDuration * 0.85))
                continue;
            accepted.Add(candidate);
            if (accepted.Count >= desiredCount) break;
        }

        return (accepted.OrderBy(m => m.Start).ToList(), null);
    }
    catch (TaskCanceledException)
    {
        return ([], "El análisis automático tardó demasiado. Prueba con un video más corto o menos clips.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error en análisis automático de clips: " + ex.Message);
        return ([], "No se pudo analizar automáticamente el video: " + ex.Message);
    }
    finally
    {
        try { if (Directory.Exists(audioDirectory)) Directory.Delete(audioDirectory, true); } catch { }
    }
}

app.MapPost("/generar", async (Solicitud solicitud) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(solicitud.Tema))
            return Results.BadRequest(new { error = "Escribe un tema." });

        string instrucciones = solicitud.Tipo switch
        {
            "Prompt para imagen" =>
                "ActÃºa como director de arte y creador de prompts. Redacta un prompt visual detallado y listo para usar en un generador de imÃ¡genes. Describe sujeto, escenario, composiciÃ³n, iluminaciÃ³n, paleta de colores, estilo, perspectiva, calidad y formato. Respeta exactamente cualquier texto que el usuario quiera incluir. Responde en espaÃ±ol.",
            "Buscador IA" =>
                "ActÃºa como el asistente de MARY. Responde en espaÃ±ol con claridad, orden y utilidad. Si la pregunta pide datos actuales o noticias, aclara que esta versiÃ³n no consulta Internet en tiempo real. No inventes bÃºsquedas web, fuentes, enlaces ni hechos. Distingue hechos de recomendaciones y reconoce incertidumbre.",
            "Crear prompts" =>
                "ActÃºa como especialista en ingenierÃ­a de prompts. Crea o mejora un prompt claro, especÃ­fico y reutilizable para la herramienta de IA que indique el usuario. Incluye objetivo, contexto, instrucciones, estilo, formato y restricciones necesarias. Si falta el nombre de la herramienta, crea un prompt adaptable. Responde en espaÃ±ol.",
            "Guion de video con audio" =>
                "ActÃºa como productor audiovisual. Crea un plan de video en espaÃ±ol con tÃ­tulo, gancho inicial, escenas con duraciÃ³n, descripciÃ³n visual de cada escena, narraciÃ³n exacta para voz en off, texto en pantalla, sugerencias de mÃºsica y efectos de sonido, y cierre. Adapta la estructura a la duraciÃ³n y formato que indique el usuario. No afirmes que has renderizado ni creado un archivo MP4 o audio; entrega el guion y las instrucciones de producciÃ³n.",
            "ReflexiÃ³n motivacional" =>
                "Escribe una reflexiÃ³n original en espaÃ±ol sobre el tema indicado. Hazla emotiva, natural y fÃ¡cil de narrar. Incluye un comienzo que atrape, una idea central y un cierre memorable. Respeta la duraciÃ³n solicitada.",
            "Podcast" =>
                "Crea un guion de podcast en espaÃ±ol con tÃ­tulo, gancho inicial, presentaciÃ³n, desarrollo con ideas claras, transiciones y cierre. Usa un tono conversacional y respeta la duraciÃ³n solicitada.",
            "Historia o frutinovela" =>
                "Crea una historia original en espaÃ±ol con personajes definidos, conflicto, diÃ¡logos naturales, tensiÃ³n, escenas visuales y un final sorprendente o emotivo. Divide en escenas si ayuda al formato solicitado.",
            "Guion para TikTok y Facebook" =>
                "Crea un guion para TikTok y Facebook en espaÃ±ol. Incluye un gancho fuerte para los primeros segundos, escenas breves, narraciÃ³n, texto en pantalla y un cierre que invite de forma natural a comentar o seguir la pÃ¡gina. Adapta la duraciÃ³n indicada.",
            _ => "Tipo de solicitud no disponible. Elige una herramienta del Buscador IA."
        };

        string prompt = instrucciones + "\n\nTema del usuario: " + solicitud.Tema +
                        "\n\nResponde solamente con el contenido solicitado en espaÃ±ol.";

        string? groqKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");
        string? openRouterKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");

        using var cliente = new HttpClient { Timeout = TimeSpan.FromSeconds(75) };

        async Task<(string? texto, string error)> ConsultarProveedor(
            string url, string clave, string modelo)
        {
            try
            {
                var cuerpo = new
                {
                    model = modelo,
                    messages = new[]
                    {
                        new { role = "system", content = "Eres MARY, una asistente Ãºtil que responde en espaÃ±ol." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7
                };

                using var peticion = new HttpRequestMessage(HttpMethod.Post, url);
                peticion.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clave);
                peticion.Content = new StringContent(
                    JsonSerializer.Serialize(cuerpo), Encoding.UTF8, "application/json");

                using var respuesta = await cliente.SendAsync(peticion);
                string contenido = await respuesta.Content.ReadAsStringAsync();

                if (!respuesta.IsSuccessStatusCode)
                    return (null, $"HTTP {(int)respuesta.StatusCode}: {contenido}");

                using var documento = JsonDocument.Parse(contenido);

                if (!documento.RootElement.TryGetProperty("choices", out var choices) ||
                    choices.GetArrayLength() == 0 ||
                    !choices[0].TryGetProperty("message", out var message) ||
                    !message.TryGetProperty("content", out var content))
                    return (null, "El proveedor no devolviÃ³ contenido vÃ¡lido.");

                string? texto = content.GetString();

                return !string.IsNullOrWhiteSpace(texto)
                    ? (texto, "")
                    : (null, "El proveedor devolviÃ³ una respuesta vacÃ­a.");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        if (!string.IsNullOrWhiteSpace(groqKey))
        {
            Console.WriteLine("MARY: intentando generar con Groq...");
            var groq = await ConsultarProveedor(
                "https://api.groq.com/openai/v1/chat/completions",
                groqKey,
                "openai/gpt-oss-20b");

            if (groq.texto != null)
            {
                Console.WriteLine("MARY: contenido generado con Groq.");
                return Results.Ok(new { texto = groq.texto, proveedor = "Groq" });
            }

            Console.WriteLine("Groq no disponible; intentando respaldo gratuito.");
        }

        if (!string.IsNullOrWhiteSpace(openRouterKey))
        {
            Console.WriteLine("MARY: intentando generar con OpenRouter gratuito...");
            var respaldo = await ConsultarProveedor(
                "https://openrouter.ai/api/v1/chat/completions",
                openRouterKey,
                "openrouter/free");

            if (respaldo.texto != null)
            {
                Console.WriteLine("MARY: contenido generado con OpenRouter.");
                return Results.Ok(new { texto = respaldo.texto, proveedor = "OpenRouter gratuito" });
            }
        }

        return Results.Problem(
            detail: "MARY no pudo generar el contenido. Revisa las claves y los lÃ­mites gratuitos de Groq y OpenRouter.",
            statusCode: 502);
    }
    catch (TaskCanceledException)
    {
        return Results.Problem(detail: "La solicitud tardÃ³ demasiado. IntÃ©ntalo de nuevo.", statusCode: 504);
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR AL GENERAR: " + ex.Message);
        return Results.Problem(detail: "No se pudo generar el contenido. " + ex.Message, statusCode: 500);
    }
});

static async Task<string?> ValidateRemoteMediaUrlAsync(string rawUrl, string mode)
{
    if (string.IsNullOrWhiteSpace(rawUrl) || rawUrl.Length > 2048 || !Uri.TryCreate(rawUrl, UriKind.Absolute, out var uri))
        return "Escribe una URL completa y válida.";

    string scheme = uri.Scheme.ToLowerInvariant();
    if (uri.UserInfo.Length > 0)
        return "No pegues enlaces que contengan usuario o contraseña.";
    if (scheme is not ("http" or "https" or "rtmp" or "rtmps"))
        return "Protocolo no admitido. Usa HTTP/HTTPS para archivos y HLS, o RTMP/RTMPS para una señal en vivo.";

    bool portAllowed = scheme switch
    {
        "http" => uri.Port == 80,
        "https" => uri.Port == 443,
        "rtmp" => uri.Port == 1935,
        "rtmps" => uri.Port == 443,
        _ => false
    };
    if (!portAllowed)
        return "El puerto de ese enlace no está permitido por seguridad. Usa HTTP 80, HTTPS 443, RTMP 1935 o RTMPS 443.";

    if (uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || uri.Host.EndsWith(".local", StringComparison.OrdinalIgnoreCase))
        return "No se permiten direcciones locales o privadas.";

    try
    {
        IPAddress[] addresses;
        if (IPAddress.TryParse(uri.DnsSafeHost, out var literalAddress))
            addresses = [literalAddress];
        else
            addresses = await Dns.GetHostAddressesAsync(uri.DnsSafeHost);

        if (addresses.Length == 0 || addresses.Any(address => !IsPublicInternetAddress(address)))
            return "El servidor del enlace no parece tener una dirección pública. Por seguridad, MARY no puede conectarse a redes privadas.";
    }
    catch
    {
        return "MARY no pudo resolver el servidor de ese enlace. Comprueba la dirección e inténtalo de nuevo.";
    }

    string path = uri.AbsolutePath.ToLowerInvariant();
    string extension = Path.GetExtension(path);
    if (mode == "directo")
    {
        if (scheme is not ("http" or "https"))
            return "Para un archivo directo usa un enlace HTTP o HTTPS.";
        if (extension is not (".mp4" or ".mov" or ".m4v" or ".webm" or ".mkv" or ".avi" or ".m3u8"))
            return "Ese enlace no parece un archivo de video directo. Se admiten MP4, MOV, WEBM, MKV, AVI o una lista HLS .m3u8.";
    }
    else if (mode == "live")
    {
        bool hlsLive = (scheme is "http" or "https") && extension == ".m3u8";
        bool rtmpLive = (scheme is "rtmp" or "rtmps");
        if (!hlsLive && !rtmpLive)
            return "Para LIVE usa una URL HLS terminada en .m3u8 o una señal RTMP/RTMPS.";
    }

    return null;
}

static bool IsPublicInternetAddress(IPAddress address)
{
    if (address.IsIPv4MappedToIPv6)
        address = address.MapToIPv4();
    if (IPAddress.IsLoopback(address) || address.IsIPv6LinkLocal || address.IsIPv6SiteLocal || address.IsIPv6Multicast)
        return false;

    byte[] b = address.GetAddressBytes();
    if (address.AddressFamily == AddressFamily.InterNetwork)
    {
        if (b[0] == 0 || b[0] == 10 || b[0] == 127 || b[0] >= 224)
            return false;
        if (b[0] == 100 && b[1] >= 64 && b[1] <= 127) return false;
        if (b[0] == 169 && b[1] == 254) return false;
        if (b[0] == 172 && b[1] >= 16 && b[1] <= 31) return false;
        if (b[0] == 192 && b[1] == 168) return false;
        if (b[0] == 192 && b[1] == 0 && b[2] == 0) return false;
        if (b[0] == 192 && b[1] == 0 && b[2] == 2) return false;
        if (b[0] == 198 && (b[1] == 18 || b[1] == 19)) return false;
        if (b[0] == 198 && b[1] == 51 && b[2] == 100) return false;
        if (b[0] == 203 && b[1] == 0 && b[2] == 113) return false;
        return true;
    }

    if (address.AddressFamily == AddressFamily.InterNetworkV6)
    {
        // Solo permite unicast global 2000::/3 y bloquea el prefijo de documentación.
        if ((b[0] & 0xE0) != 0x20) return false;
        if (b[0] == 0x20 && b[1] == 0x01 && b[2] == 0x0D && b[3] == 0xB8) return false;
        return true;
    }

    return false;
}

app.Run();

public record ClipMoment(double Start, double End, string Category, string Reason, int Potential);
public record Solicitud(string Tipo, string Tema);
