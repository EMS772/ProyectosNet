class Slideshow {
    constructor(el) {
        this.DOM = { el: el };
        this.config = {
            slideshow: {
                pagination: {
                    duration: 3,
                }
            }
        };
        this.init();
    }

    init() {
        this.DOM.slideTitle = this.DOM.el.querySelectorAll('.slide-title');
        this.DOM.slideTitle.forEach((slideTitle) => {
            this.splitText(slideTitle);
        });
        this.initSlider();
        this.initButtonEvents();
    }

    splitText(element) {
        const letters = element.innerText.split('');
        element.innerHTML = letters.map(letter => `<span>${letter}</span>`).join('');
    }

    initSlider() {
        this.slideshow = new Swiper(this.DOM.el, {
            loop: false,
            speed: 1000,
            preloadImages: true,
            updateOnImagesReady: true,
            slidesPerView: 1,
            slidesPerGroup: 1,
            loopAdditionalSlides: 2,
            pagination: {
                el: '.slideshow-pagination',
                clickable: true,
                bulletClass: 'slideshow-pagination-item',
                bulletActiveClass: 'active',
                clickableClass: 'slideshow-pagination-clickable',
                modifierClass: 'slideshow-pagination-',
                renderBullet: function (index, className) {
                    var slideIndex = index,
                        number = (index <= 8) ? '0' + (slideIndex + 1) : (slideIndex + 1);
                    var paginationItem = '<span class="slideshow-pagination-item">';
                    paginationItem += '<span class="pagination-number">' + number + '</span>';
                    paginationItem = (index <= 8) ? paginationItem + '<span class="pagination-separator"><span class="pagination-separator-loader"></span></span>' : paginationItem;
                    paginationItem += '</span>';
                    return paginationItem;
                },
            },
            on: {
                init: () => {
                    this.animate('next');
                },
                slideChangeTransitionStart: () => {
                    this.animate('next');
                }
            }
        });
    }

    initButtonEvents() {
        // Seleccionar todos los botones de "Next Slide"
        const nextButtons = this.DOM.el.querySelectorAll('.slide-next, .btn-light');
        nextButtons.forEach(button => {
            button.addEventListener('click', () => {
                console.log('Botón clickeado, avanzando al siguiente slide');
                this.slideshow.slideNext();
            });
        });
    }

    animate(direction = 'next') {
        this.DOM.activeSlide = this.DOM.el.querySelector('.swiper-slide-active');
        this.DOM.activeSlideImg = this.DOM.activeSlide.querySelector('.slide-image');
        this.DOM.activeSlideTitle = this.DOM.activeSlide.querySelector('.slide-title');
        this.DOM.activeSlideTitleLetters = this.DOM.activeSlideTitle ? this.DOM.activeSlideTitle.querySelectorAll('span') : [];

        if (this.DOM.activeSlideImg) {
            TweenMax.set(this.DOM.activeSlideImg, { x: 0 });
            TweenMax.to(this.DOM.activeSlideImg, 1, {
                ease: 'Power2.easeInOut',
                startAt: { scale: 1.2 },
                scale: 1
            });
        }

        if (this.DOM.activeSlideTitleLetters.length > 0) {
            TweenMax.set(this.DOM.activeSlideTitleLetters, { y: '50%', opacity: 0 });
            this.DOM.activeSlideTitleLetters.forEach((letter, pos) => {
                TweenMax.to(letter, 0.6, {
                    ease: 'Back.easeOut',
                    delay: pos * 0.05,
                    y: '0%',
                    opacity: 1
                });
            });
        }
    }
}

// Inicializar el slideshow cuando el DOM esté completamente cargado
document.addEventListener('DOMContentLoaded', () => {
    const slideshowElement = document.querySelector('.slideshow');
    if (slideshowElement) {
        new Slideshow(slideshowElement);
    } else {
        console.error('No se encontró el elemento .slideshow');
    }
});


//####################################################
//####################################################
//####################################################
//####################################################

$(function () {

    var EASE = Power4.easeOut;

    var Engine = {
        ui: {
            initBtn: function () {
                var card = $('.card, .btn');
                var body = $('body');
                var btn = $('.btn');

                card.on('click', function () {

                    if (body.hasClass('is-open')) {
                        body.removeClass('is-open');
                    } else {
                        body.addClass('is-open');
                        TweenMax.set('.card', { clearProps: 'all' });
                    }
                })
            },
            initHover: function (e) {
                $(document).on("mousemove", ".card", function (e) {
                    if ($('body').hasClass('is-open')) {
                        e.preventDefault();
                        // $('.card').attr('style', '').children('.card-title-wrap').attr('style', '');
                    } else {
                        var halfW = (this.clientWidth / 2);
                        var halfH = (this.clientHeight / 2);

                        var coorX = (halfW - (event.pageX - this.offsetLeft));
                        var coorY = (halfH - (event.pageY - this.offsetTop));

                        var degX = ((coorY / halfH) * 10) + 'deg'; // max. degree = 10
                        var degY = ((coorX / halfW) * -10) + 'deg'; // max. degree = 10

                        $(this).css('transform', function () {
                            return 'perspective(1600px) translate3d(0, 0px, 0) scale(0.6) rotateX(' + degX + ') rotateY(' + degY + ')';
                        }).children('.card-title-wrap').css('transform', function () {
                            return 'perspective(1600px) translate3d(0, 0, 200px) rotateX(' + degX + ') rotateY(' + degY + ')';
                        });
                    }
                }).on("mouseout", ".card", function () {
                    $(this).removeAttr('style').children('.card-title-wrap').removeAttr('style');
                });


            }
        }
    };

    Engine.ui.initBtn();
    Engine.ui.initHover();

})

function parallax_height() {
    var scroll_top = $(this).scrollTop();
    var sample_section = $(".sample-section");
    if (sample_section.length > 0) {
        var sample_section_top = sample_section.offset().top;
        var header_height = $(".sample-header-section").outerHeight();
        sample_section.css({ "margin-top": header_height });
        $(".sample-header").css({ height: header_height - scroll_top });
    }
}
parallax_height();
$(window).scroll(function () {
    parallax_height();
});
$(window).resize(function () {
    parallax_height();
});

