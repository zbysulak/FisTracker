<template>
  <v-container fluid>
    <v-row class="text-center">
      <v-col class="mb-4">
        <h1 class="font-weight-bold mb-3">Welcome {{ userName }}</h1>

        <p class="subheading font-weight-regular">
          Insert your time manually or upload screenshot of your attendance
        </p>
      </v-col>
    </v-row>
    <div v-if="isLogged">
      <v-col cols="12">
        <time-sheet></time-sheet>
      </v-col>
    </div>
    <v-container class="text-center loginBtn" v-else>
      <h4>Please, sign in!</h4>
      <login />
      <p class="mt-5 mb-0">or</p>
      <register />
    </v-container>
  </v-container>
</template>

<script>
import TimeSheet from "../components/TimeSheet.vue"
import Login from "../components/Login.vue"
import Register from "../components/Register.vue"

export default {
  name: "Home",

  components: {
    TimeSheet,
    Login,
    Register
  },

  data: () => ({
    isLogged: false,
    userName: ""
  }),
  watch: {
    "$store.state.user": {
      handler: function () {
        if (!this.$store.state.user.token) {
          this.isLogged = false
          this.userName = ""
        } else {
          this.isLogged = true
          console.log(this.$store.state.user.name)
          this.userName = this.$store.state.user.name
        }
      },
      immediate: true // provides initial (not changed yet) state
    }
  },
  computed: {}
}
</script>

<style>

.container {
  max-width: 90%;
}

.loginBtn .v-btn {
  border: 1px solid #1976d2 !important;
  margin-right: 0 !important;
  color: #1976d2 !important;
  margin-top: 20px;
}
</style>
