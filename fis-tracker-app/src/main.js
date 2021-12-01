import Vue from "vue"
import App from "./App.vue"
import vuetify from "./plugins/vuetify"
import router from "./router"
import config from "./config"

Vue.config.productionTip = false
Vue.prototype.appConfig = config

Vue.filter("formatDate", function(date){
  let d = new Date(date)
  return d.toLocaleDateString()
})

new Vue({
  vuetify,
  router,
  render: (h) => h(App)
}).$mount("#app")
