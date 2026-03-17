/**
 * portfolio.js  —  wwwroot/js/portfolio.js
 *
 * Loaded by index.html. Called from Home.razor on first render:
 *   await JS.InvokeVoidAsync("portfolioInit");
 */
window.portfolioInit = function () {

    // ── CUSTOM CURSOR ──────────────────────────────────────────
    const cursor     = document.getElementById('cursor');
    const cursorRing = document.getElementById('cursorRing');

    if (cursor && cursorRing) {
        let mx = 0, my = 0, rx = 0, ry = 0;

        document.addEventListener('mousemove', e => {
            mx = e.clientX; my = e.clientY;
            cursor.style.left = mx + 'px';
            cursor.style.top  = my + 'px';
        });

        (function animateRing() {
            rx += (mx - rx) * 0.12;
            ry += (my - ry) * 0.12;
            cursorRing.style.left = rx + 'px';
            cursorRing.style.top  = ry + 'px';
            requestAnimationFrame(animateRing);
        })();

        document.querySelectorAll('a, button, .skill-card, .highlight-card, .tool-cell, .project-card')
            .forEach(el => {
                el.addEventListener('mouseenter', () => {
                    cursor.style.width       = '20px';
                    cursor.style.height      = '20px';
                    cursorRing.style.width   = '60px';
                    cursorRing.style.height  = '60px';
                    cursorRing.style.opacity = '0.7';
                });
                el.addEventListener('mouseleave', () => {
                    cursor.style.width       = '12px';
                    cursor.style.height      = '12px';
                    cursorRing.style.width   = '40px';
                    cursorRing.style.height  = '40px';
                    cursorRing.style.opacity = '0.4';
                });
            });
    }

    // ── REVEAL ON SCROLL ───────────────────────────────────────
    const revealObs = new IntersectionObserver(entries => {
        entries.forEach(e => { if (e.isIntersecting) e.target.classList.add('visible'); });
    }, { threshold: 0.15 });

    document.querySelectorAll('.reveal, .timeline-item').forEach(el => revealObs.observe(el));

    // ── EXPERTISE BARS ─────────────────────────────────────────
    const barObs = new IntersectionObserver(entries => {
        entries.forEach(e => {
            if (!e.isIntersecting) return;
            e.target.querySelectorAll('.exp-bar').forEach(bar => {
                bar.style.width = bar.dataset.width + '%';
            });
            barObs.unobserve(e.target);
        });
    }, { threshold: 0.3 });

    document.querySelectorAll('.expertise-list').forEach(el => barObs.observe(el));

    // ── NAV SCROLL TINT ────────────────────────────────────────
    const nav = document.querySelector('nav');
    if (nav) {
        window.addEventListener('scroll', () => {
            nav.style.background = window.scrollY > 60
                ? 'rgba(10,10,15,0.97)'
                : '';
        }, { passive: true });
    }

    // ── SMOOTH ANCHOR SCROLL ───────────────────────────────────
    document.querySelectorAll('a[href^="#"]').forEach(a => {
        a.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href === '#') return;
            const target = document.querySelector(href);
            if (target) { e.preventDefault(); target.scrollIntoView({ behavior: 'smooth' }); }
        });
    });
};