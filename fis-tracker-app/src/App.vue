<template>
  <v-app>
    <v-overlay :value="isLoading">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
    <v-app-bar app color="primary" dark>
      <div class="d-flex align-center">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="https://cdn.vuetifyjs.com/images/logos/vuetify-logo-dark.png"
          transition="scale-transition"
          width="40" />

        <span class="font-weight-bold">FisTracker</span>
      </div>
      <v-spacer></v-spacer>
      <v-row class="justify-end align-center">
        <div class="text-center">
          <v-dialog v-model="dialog" width="800">
            <template v-slot:activator="{ on, attrs }">
              <v-btn v-bind="attrs" v-on="on" class="mr-5" text> FAQ </v-btn>
            </template>

            <v-card>
              <v-card-title class="text-h5 grey lighten-2"> FAQ </v-card-title>

              <v-card-text class="pt-5">
                <faq />
              </v-card-text>

              <v-divider></v-divider>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="primary" text @click="dialog = false">
                  Close
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </div>
        <settings v-if="isLogged" />
        <Login />
      </v-row>
    </v-app-bar>
    <v-main v-if="!isLoading">
      <router-view></router-view>
      <div class="faq mt-5" v-if="!isLogged">
        <h3 class="mb-2">How it works</h3>
        <faq />
      </div>
    </v-main>
    <v-footer padless color="white">
      <Footer />
    </v-footer>
    <snack ref="snack" />
  </v-app>
</template>

<script>
import Login from "./components/Login"
import Settings from "./components/Settings.vue"
import Snack from "./components/Snack.vue"
import axios from "axios"
import Footer from "./components/Footer.vue"
import Faq from "./components/Faq.vue"

export default {
  name: "App",
  components: { Login, Settings, Snack, Footer, Faq },

  data: () => ({
    dialog: false,
    isLogged: false,
    isLoading: true
  }),

  beforeCreate() {
    const url = this.appConfig.apiUrl + "/Users/AuthorizationCheck"
    axios.defaults.headers = { Authorization: window.localStorage.token }
    axios
      .get(url)
      .then((res) => {
        if (res.data.token !== undefined) {
          const t = window.localStorage.token
          this.$store.state.user = { token: t, name: res.data.name }
          this.isLogged = true
        } else {
          this.$store.state.user = { token: undefined }
          this.isLogged = false
          console.log("UNDEFINED TOKEN")
        }
      })
      .catch((err) => {
        console.log("CHYBA", err)
        this.$store.state.user = { token: undefined }
      })
      .finally(() => {
        this.isLoading = false
      })
  },

  mounted() {
    this.$root.snack = this.$refs.snack
  },
  watch: {
    "$store.state.user": {
      handler: function () {
        axios.defaults.headers = { Authorization: this.$store.state.user.token }

        if (this.$store.state.user.token === undefined) {
          this.isLogged = false
        } else {
          this.isLogged = true
        }
      },
      immediate: true // provides initial (not changed yet) state
    }
  }
}
</script>

<style>
.faq {
  width: 60%;
  margin-left: auto;
  margin-right: auto;
}
.faq h3 {
  text-align: center;
}


@media only screen and (min-width: 601px) {
  
  .desktop_none {
    display: none;
  }

}

@media only screen and (max-width: 600px) {

  .container {
    max-width: 95% !important;
  }

  .mobile_none {
    display: none !important;
  }

  .mobile_block {
    display: block !important;
  }
  
  .faq {
    width: 80%;
    margin-bottom: 50px;
  }

  .display-2 {
    font-size: 2rem !important;
  }

}

</style>
