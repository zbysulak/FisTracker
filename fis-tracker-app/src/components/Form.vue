<template>
  <form>
    <v-row>
      <v-menu
        ref="menu"
        v-model="menu"
        :close-on-content-click="false"
        :return-value.sync="date"
        transition="scale-transition"
        offset-y
        min-width="auto"
      >
        <template v-slot:activator="{ on, attrs }">
          <v-text-field
            v-model="date"
            label="Select date"
            prepend-icon="mdi-calendar"
            required
            @input="$v.date.$touch()"
            @blur="$v.date.$touch()"
            readonly
            v-bind="attrs"
            v-on="on"
            :error-messages="dateError"
            class="mr-4"
          ></v-text-field>
        </template>
        <v-date-picker v-model="date" no-title scrollable>
          <v-spacer></v-spacer>
          <v-btn text color="primary" @click="menu = false"> Cancel </v-btn>
          <v-btn text color="primary" @click="$refs.menu.save(date)">
            OK
          </v-btn>
        </v-date-picker>
      </v-menu>
      <v-text-field
        v-model="timeFrom"
        :error-messages="timeFromError"
        label="From"
        required
        @input="$v.timeFrom.$touch()"
        @blur="$v.timeFrom.$touch()"
        class="mr-4"
      ></v-text-field>
      <v-text-field
        v-model="timeTo"
        :error-messages="timeToError"
        label="To"
        required
        @input="$v.timeTo.$touch()"
        @blur="$v.timeTo.$touch()"
        class="mr-4"
      ></v-text-field>
      <v-btn class="mr-4" @click="submit"> submit </v-btn>
      <v-btn @click="clear"> clear </v-btn>
    </v-row>
  </form>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";

export default {
  name: "Form",

  components: {},

  mixins: [validationMixin],

  validations: {
    date: { required },
    timeFrom: { required },
    timeTo: { required },
  },

  data: () => ({
    timeFrom: "",
    timeTo: "",
    date: new Date(Date.now() - new Date().getTimezoneOffset() * 60000)
      .toISOString()
      .substr(0, 10),
    menu: false,
    modal: false,
    menu2: false,
  }),

  computed: {
    dateError() {
      const errors = [];

      if (!this.$v.date.$dirty) return errors;
      !this.$v.date.required && errors.push("Date is required.");

      return errors;
    },

    timeFromError() {
      const errors = [];
      if (!this.$v.timeFrom.$dirty) return errors;
      !this.$v.timeFrom.required && errors.push("Time is required.");

      return errors;
    },

    timeToError() {
      const errors = [];
      if (!this.$v.timeTo.$dirty) return errors;
      !this.$v.timeTo.required && errors.push("Time is required.");

      return errors;
    },
  },

  methods: {
    submit() {
      this.$v.$touch();
    },
    clear() {
      this.$v.$reset();
      this.date = "";
      this.timeFrom = "";
      this.timeTo = "";
    },
  },
};
</script>
