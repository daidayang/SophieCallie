$(document).ready(function () {
    function queueOptimizelyCampaignData(event) {
        window.optimizelyCampaignsQueue.push(event);
    };

    window.optimizely = window.optimizely || [];
    window.optimizelyCampaignsQueue = [];
    window.optimizely.push({
        type: "addListener",
        filter: {
            type: "lifecycle",
            name: "campaignDecided"
        },
        handler: queueOptimizelyCampaignData,
        token: "queueOptimizelyCampaignDecisions"
    });
})();

window.amplitudeQueue = window.amplitudeQueue || [];

window['GoogleAnalyticsObject'] = 'ga';
window['ga'] = window['ga'] || function () {
    (window['ga'].q = window['ga'].q || []).push(arguments)
};

if ("undefined" === typeof google_tag_manager["GTM-TRHD3G"].macro(1)) {
    var _fbq = window._fbq || (window._fbq = []); if (!_fbq.loaded) {
        var fbds = document.createElement("script");
        fbds.async = !0; fbds.src = "//connect.facebook.net/en_US/fbds.js";
        var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(fbds, s); _fbq.loaded = !0
    } _fbq.push(["addPixelId", "609729382476743"]); window._fbq = window._fbq || [];
    window._fbq.push(["track", "PixelInitialized", {}])
} document.cookie = "fbtagfired\x3d1";