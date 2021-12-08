<template>
  <div>
    <v-dialog v-model="dialog" persistent max-width="600px">
      <template v-slot:activator="{ on, attrs }">
        <v-btn class="mr-10" color="white" outlined v-bind="attrs" v-on="on">
          Login
          <v-icon>mdi-login</v-icon>
        </v-btn>
      </template>
      <v-card>
        <v-form @submit.prevent="login">
          <v-card-title>
            <span class="text-h5">Login</span>
          </v-card-title>
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12">
                  <v-text-field
                    v-model="name"
                    :error-messages="nameError"
                    label="Name"
                    required
                    @input="$v.name.$touch()"
                    @blur="$v.name.$touch()"
                    class="mr-4"></v-text-field>
                </v-col>
                <v-col cols="12">
                  <v-text-field
                    v-model="password"
                    :error-messages="passError"
                    label="Password"
                    type="password"
                    required
                    @input="$v.password.$touch()"
                    @blur="$v.password.$touch()"
                    class="mr-4"></v-text-field>
                </v-col>
              </v-row>
            </v-container>
            <small>*indicates required field</small>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="black darken-1" text @click="dialog = false">
              Close
            </v-btn>
            <v-btn color="blue darken-1" text @click="authTest">
              Auth test
            </v-btn>
            <v-btn color="primary" type="submit"> Log in </v-btn>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
    <v-snackbar v-model="snackbar" :timeout="timeout">
      {{ text }}

      <template v-slot:action="{ attrs }">
        <v-btn color="blue" text v-bind="attrs" @click="snackbar = false">
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script>
import { validationMixin } from "vuelidate"
import { required } from "vuelidate/lib/validators"
import axios from "axios"

export default {
  name: "Login",

  components: {},

  mixins: [validationMixin],

  validations: {
    name: { required },
    password: { required }
  },

  data: () => ({
    snackbar: false,
    text: "Successfully log in.",
    timeout: 2000,
    dialog: false,
    name: process.env.NODE_ENV === "production" ? "" : "Martin",
    password: process.env.NODE_ENV === "production" ? "" : "Martin",
    error: null,
    success: false
  }),
  computed: {
    nameError() {
      const errors = []
      if (!this.$v.name.$dirty) return errors
      !this.$v.name.required && errors.push("Name is required.")

      return errors
    },

    passError() {
      const errors = []
      if (!this.$v.password.$dirty) return errors
      !this.$v.password.required && errors.push("Password is required.")

      return errors
    }
  },

  methods: {
    submit() {
      this.$v.$touch()
    },

    authTest: async function () {
      const url = this.appConfig.apiUrl + "/Test/Auth"
      this.success = false
      this.error = null

      try {
        const res = await axios
          .get(url, { withCredentials: true })
          .then((res) => res.data)
        this.success = true
        this.dialog = false
        this.snackbar = true
        console.log(res)
      } catch (err) {
        this.error = err.message
        console.log(this.error)
      }
    },

    login: async function () {
      this.nameError
      this.passError
      console.log(this.name + " " + this.password)
      const auth = { name: this.name, password: this.password }
      const url = this.appConfig.apiUrl + "/Users/login"
      this.success = false
      this.error = null

      try {
        const res = await axios
          .post(url, auth, { withCredentials: true })
          .then((res) => res.data)
        this.success = true
        this.dialog = false
        this.snackbar = true
        this.$store.commit("TEST")
        console.log("JUCHUUU")
        console.log(res)
      } catch (err) {
        this.error = err.message
        console.log(this.error)
      }
    }
  }
}
</script>
