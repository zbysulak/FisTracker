<template>
  <v-menu
    v-model="datePicker"
    :close-on-content-click="false"
    :nudge-right="40"
    transition="scale-transition"
    offset-y
    min-width="auto">
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        :label="label"
        prepend-icon="mdi-calendar"
        readonly
        v-bind="attrs"
        v-on="on"
        v-model="date"></v-text-field>
    </template>
    <v-date-picker v-model="date" @input="emit"></v-date-picker>
  </v-menu>
</template>
<script>
export default {
  props: {
    label: { type: String, default: "Date" },
    value: { type: String, required: "true" }
  },
  data: () => ({
    datePicker: false,
    date: ""
  }),
  methods: {
    emit: function () {
      this.datePicker = false
      this.$emit("input", this.date)
    }
  },
  watch: {
    value(val) {
      this.date = val.toString().substring(0, 10)
    }
  },
  mounted: function () {
    this.date = this.value.toString().substring(0, 10)
  }
}
</script>
